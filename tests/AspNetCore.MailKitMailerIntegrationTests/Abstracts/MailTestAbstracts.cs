using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using netDumbster.smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.MailKitMailer;
using AspNetCore.MailKitMailer.Models;
using Newtonsoft.Json;
using System.Text;
using HtmlAgilityPack;
using Xunit;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AspNetCore.MailKitMailerIntegrationTests.Abstracts
{
    public abstract class MailTestAbstracts:IDisposable
    {
        protected IServiceProvider serviceProvider;
        protected readonly IHost server;
        protected readonly HttpClient client;
        protected SimpleSmtpServer mailServer;
        static WebApplication dlServer;

        public MailTestAbstracts()
        {
            // Create smtp server
            this.mailServer = SimpleSmtpServer.Start();

            // Create Test Web Server
            var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                // Use Kestrel so we can int test real downloads
                webHost.UseTestServer();
                webHost.UseStartup<IntegrationTestsWebApp.Startup>();

                // configure the services after the startup has been called.
                webHost.ConfigureTestServices(services =>
                {
                    services.AddAspNetCoreMailKitMailer(
                       new SMTPConfigModel()
                       {
                           CheckCertificateRevocation = false,
                           DoAuthenticate = false,
                           FromAddress = new EmailAddressModel("Root", "root@localhost"),
                           Host = "localhost",
                           Port = this.mailServer.Configuration.Port,
                           UseSSL = false
                       }, smtpClient => {
                           smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                       });

                    services.RegisterAllMailContexOfAssemblyContainingType<IntegrationTestsWebApp.Startup>();

                });

            });

            this.server = hostBuilder.StartAsync().Result;
            this.client = this.server.GetTestClient();

            ServiceCollection services = new ServiceCollection();
         


            this.serviceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        /// Starts an slim web server to test file downloadds
        /// Remember to close it when used!
        /// </summary>
        /// <returns></returns>
        protected IHost StartDownloadServer()
        {

            // Create Download Server 
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://localhost:3333/");

            var app = builder.Build();
           
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.MapGet("/dl/{name}.{ext}", async httpContext =>
            {
                string n = httpContext.Request.RouteValues["name"].ToString();
                string ex = httpContext.Request.RouteValues["ext"].ToString();

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "inttest.txt");

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {

                    fs.Close();
                }

                File.WriteAllText(filePath, "TestDownload");

                await httpContext.Response.SendFileAsync(filePath);
            });


            app.MapGet("/dl2/{name}", async httpContext =>
            {
                string n = httpContext.Request.RouteValues["name"].ToString();

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{n}.txt");

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {

                    fs.Close();
                }

                File.WriteAllText(filePath, "TestDownload2");
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = n + ".txt",
                    Inline = false  // false = prompt the user for downloading;  true = browser to try to show the file inline
                };

                httpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
                httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                await httpContext.Response.SendFileAsync(filePath);
            });

            app.Start();

            dlServer = app;

            return app;
        }

        protected async Task StopDownloadServer()
        {
            if (dlServer != null)
            {
                await dlServer.StopAsync();
                await dlServer.DisposeAsync();
                dlServer = null;
            }
        }

        protected HttpContent MakeContent<T>(T content) where T:class
        {
            string json = JsonConvert.SerializeObject(content); // or JsonSerializer.Serialize if using System.Text.Json


            StringContent stringContent = 
                new StringContent(
                    json, 
                    UnicodeEncoding.UTF8, 
                    "application/json"
                    ); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

            return stringContent;
        }

        protected void assertBody(HtmlDocument doc, EmailAddressModel addr, string prefix)
        {
            var addrNodes = doc.DocumentNode.SelectNodes($"//li[contains(@class, '{prefix}')]");
            Assert.NotEmpty(addrNodes);
            List<string> foundInBody = new List<string>(); 

            foreach(var addrNode in addrNodes)
            {
                ;

                string name = addrNode.Descendants()
                    .Where(x => x.HasClass($"{prefix}_name")).FirstOrDefault()?.InnerText;

                string email = addrNode.Descendants()
                    .Where(x => x.HasClass($"{prefix}_email")).FirstOrDefault()?.InnerText;;

                foundInBody.Add($"<{name}> {email}");
            }

            Assert.Contains($"<{addr.Name}> {addr.Email}",  foundInBody);
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.server.StopAsync().Wait();
            this.server.Dispose();
            this.mailServer.Stop();
            this.mailServer.Dispose();
        }
    }
}

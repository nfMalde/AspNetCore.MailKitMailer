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

namespace AspNetCore.MailKitMailerIntegrationTests.Abstracts
{
    public abstract class MailTestAbstracts:IDisposable
    {
        protected IServiceProvider serviceProvider;
        protected readonly TestServer server;
        protected readonly HttpClient client;
        protected SimpleSmtpServer mailServer;
        public MailTestAbstracts()
        {
            // Create smtp server
            this.mailServer = SimpleSmtpServer.Start();

            // Create Test Web Server
 

            this.server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => {
                    services.AddAspNetCoreMailKitMailer(
                        new SMTPConfigModel()
                        {
                            CheckCertificateRevocation = false,
                            DoAuthenticate = false,
                            FromAddress = new EmailAddressModel("Root", "root@localhost"),
                            Host = "localhost",
                            Port = this.mailServer.Configuration.Port,
                            UseSSL = false
                        }, smtpClient  => {
                            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                        });

                    services.RegisterAllMailContexOfAssemblyContainingType<IntegrationTestsWebApp.Startup>();
                    
                })
           .UseStartup<IntegrationTestsWebApp.Startup>());

            this.client = this.server.CreateClient();

         


            ServiceCollection services = new ServiceCollection();
         


            this.serviceProvider = services.BuildServiceProvider();
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
            this.server.Dispose();
            this.mailServer.Stop();
            this.mailServer.Dispose();
        }
    }
}

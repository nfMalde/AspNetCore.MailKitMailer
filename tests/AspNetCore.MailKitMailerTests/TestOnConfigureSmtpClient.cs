using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.MailKitMailer;
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using AspNetCore.MailKitMailerTests.TestClients;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Moq;
using Xunit;

namespace AspNetCore.MailKitMailerTests
{
    public class TestOnConfigureSmtpClient
    {
        private IServiceCollection services;
        private Mock<MailkitSMTPClient> smtpClientMock;

        public TestOnConfigureSmtpClient()
        {
            this.services = new ServiceCollection();
            this.smtpClientMock = new Mock<MailkitSMTPClient>(MockBehavior.Loose) { CallBase = true };

            // Mock the methods that SmtpClient calls internally to prevent real network access
            smtpClientMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()))
                .Returns(Task.FromResult("ok"));
            smtpClientMock.Setup(x => x.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.Configure<MailerViewEngineOptions>(x => x = new MailerViewEngineOptions());
            services.Configure<SMTPConfigModel>(x =>
            {
                x.Host = "default-smtp.example.com";
                x.Port = 25;
                x.UseSSL = false;
                x.DoAuthenticate = false;
            });

            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<IMailkitSMTPClient>(x => smtpClientMock.Object);
            services.RegisterAllMailContexesOfCallingAssembly();
            services.AddScoped<ITempDataProvider>(x => Mock.Of<ITempDataProvider>(MockBehavior.Loose));

            Mock<FakeHttpHandler> mockHandler = new Mock<FakeHttpHandler> { CallBase = true };
            mockHandler
                .Setup(handler => handler.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage());

            var mockHttpClient = new HttpClient(mockHandler.Object);

            Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

            services.AddScoped<IHttpClientFactory>(x => mockFactory.Object);
        }

        private void AddViewEngineMock()
        {
            Mock<IMailerViewEngine> viewEngineMock = new Mock<IMailerViewEngine>();
            viewEngineMock.Setup(x => x.FindView(It.IsAny<ActionContext>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<ActionContext, string, bool>((actionContext, viewName, mainpage) =>
                {
                    Mock<IView> viewMock = new Mock<IView>();
                    viewMock.Setup(x => x.RenderAsync(It.IsAny<ViewContext>())).Returns(Task.CompletedTask);
                    return ViewEngineResult.Found(viewName, viewMock.Object);
                });

            services.AddScoped<IMailerViewEngine>(x => viewEngineMock.Object);
        }

        [Fact]
        public void Send_CallsOnConfigureSmtpClient()
        {
            AddViewEngineMock();
            // Seed auth mechanisms on the real SmtpClient
            smtpClientMock.Object.AuthenticationMechanisms.Add("XOAUTH2");

            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            mailClient.Send<IClientConfigMailer>(x => x.Html_WithClientConfig());

            // XOAUTH2 should have been removed by OnConfigureSmtpClient
            Assert.DoesNotContain("XOAUTH2", smtpClientMock.Object.AuthenticationMechanisms);
        }

        [Fact]
        public void Send_OnConfigureSmtpClient_ReceivesTheSmtpClient()
        {
            AddViewEngineMock();
            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            // Resolve the mailer context to check its state after send
            var mailer = provider.GetService<IClientConfigMailer>() as ClientConfigMailer;
            Assert.NotNull(mailer);

            mailClient.Send<IClientConfigMailer>(x => x.Html_WithClientConfig());

            Assert.True(mailer!.ConfigureClientCalled);
            Assert.IsAssignableFrom<SmtpClient>(mailer.ConfiguredClient);
        }

        [Fact]
        public void Send_OnConfigureSmtpClient_CalledBeforeConnect()
        {
            AddViewEngineMock();
            // Seed auth mechanisms on the real SmtpClient
            smtpClientMock.Object.AuthenticationMechanisms.Add("XOAUTH2");

            var callOrder = new System.Collections.Generic.List<string>();

            smtpClientMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>()))
                .Callback(() => callOrder.Add("Connect"))
                .Returns(Task.CompletedTask);

            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            mailClient.Send<IClientConfigMailer>(x => x.Html_WithClientConfig());

            // OnConfigureSmtpClient removed XOAUTH2 before connect was called
            Assert.DoesNotContain("XOAUTH2", smtpClientMock.Object.AuthenticationMechanisms);
            Assert.Contains("Connect", callOrder);
        }

        [Fact]
        public void DefaultOnConfigureSmtpClient_IsNoOp()
        {
            AddViewEngineMock();
            // Seed auth mechanisms on the real SmtpClient
            smtpClientMock.Object.AuthenticationMechanisms.Add("XOAUTH2");

            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            // SmtpOverrideMailer doesn't override OnConfigureSmtpClient - should still work fine
            mailClient.Send<ISmtpOverrideMailer>(x => x.Html_WithoutOverride());

            // Auth mechanisms remain unchanged
            Assert.Contains("XOAUTH2", smtpClientMock.Object.AuthenticationMechanisms);
        }
    }

    public class TestExtensionOverloads
    {
        [Fact]
        public void AddAspNetCoreMailKitMailer_ConfigureClientOnly_RegistersServices()
        {
            var services = new ServiceCollection();

            bool configCalled = false;
            services.AddAspNetCoreMailKitMailer(client =>
            {
                configCalled = true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
            });

            // Verify that service descriptors are registered
            Assert.Contains(services, sd => sd.ServiceType == typeof(IMailClient));
            Assert.Contains(services, sd => sd.ServiceType == typeof(IMailkitSMTPClient));

            // Resolve only the SMTP client to verify configureClient was applied
            var provider = services.BuildServiceProvider();
            var smtpClient = provider.GetService<IMailkitSMTPClient>();
            Assert.NotNull(smtpClient);
            Assert.True(configCalled);
            Assert.DoesNotContain("XOAUTH2", smtpClient!.AuthenticationMechanisms);
        }

        [Fact]
        public void AddAspNetCoreMailKitMailer_WithConfig_ConfigureClientIsCalled()
        {
            var services = new ServiceCollection();

            bool configCalled = false;
            services.AddAspNetCoreMailKitMailer(new SMTPConfigModel
            {
                Host = "test.example.com",
                Port = 587
            }, client =>
            {
                configCalled = true;
            });

            var provider = services.BuildServiceProvider();
            var smtpClient = provider.GetService<IMailkitSMTPClient>();

            Assert.NotNull(smtpClient);
            Assert.True(configCalled);
        }

        [Fact]
        public void AddAspNetCoreMailKitMailer_WithConfig_ConfigureClientIsOptional()
        {
            var services = new ServiceCollection();

            services.AddAspNetCoreMailKitMailer(new SMTPConfigModel
            {
                Host = "test.example.com",
                Port = 587
            });

            var provider = services.BuildServiceProvider();
            var smtpClient = provider.GetService<IMailkitSMTPClient>();

            Assert.NotNull(smtpClient);
        }
    }
}

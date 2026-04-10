using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.MailKitMailer;
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using AspNetCore.MailKitMailerTests.TestClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AspNetCore.MailKitMailerTests
{
    public class TestSmtpConfigOverride
    {
        private IServiceCollection services;
        private Mock<IMailkitSMTPClient> smtpClientMock;

        public TestSmtpConfigOverride()
        {
            this.services = new ServiceCollection();
            this.smtpClientMock = new Mock<IMailkitSMTPClient>(MockBehavior.Loose);

            services.Configure<MailerViewEngineOptions>(x => x = new MailerViewEngineOptions());
            services.Configure<SMTPConfigModel>(x =>
            {
                x.Host = "default-smtp.example.com";
                x.Port = 25;
                x.UseSSL = false;
                x.DoAuthenticate = false;
                x.CheckCertificateRevocation = false;
                x.FromAddress = new EmailAddressModel("Default Sender", "default@example.com");
            });

            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<IMailkitSMTPClient>(x => smtpClientMock.Object);
            services.RegisterAllMailContexesOfCallingAssembly();
            services.AddScoped<ITempDataProvider>(x => Mock.Of<ITempDataProvider>(MockBehavior.Loose));

            // Http client mock
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
        public void Send_WithSmtpOverride_UsesOverrideConfig()
        {
            AddViewEngineMock();
            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            mailClient.Send<ISmtpOverrideMailer>(x => x.Html_WithOverride());

            smtpClientMock.Verify(
                x => x.ConnectAsync("override-smtp.example.com", 587, true, default),
                Times.Once);
        }

        [Fact]
        public void Send_WithSmtpOverride_Authenticates()
        {
            AddViewEngineMock();
            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            mailClient.Send<ISmtpOverrideMailer>(x => x.Html_WithOverride());

            smtpClientMock.Verify(
                x => x.AuthenticateAsync("overrideuser", "overridepass", default),
                Times.Once);
        }

        [Fact]
        public void Send_WithoutOverride_UsesDefaultConfig()
        {
            AddViewEngineMock();
            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            mailClient.Send<ISmtpOverrideMailer>(x => x.Html_WithoutOverride());

            smtpClientMock.Verify(
                x => x.ConnectAsync("default-smtp.example.com", 25, false, default),
                Times.Once);
        }

        [Fact]
        public void Send_WithoutOverride_DoesNotAuthenticate()
        {
            AddViewEngineMock();
            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>()!;

            mailClient.Send<ISmtpOverrideMailer>(x => x.Html_WithoutOverride());

            smtpClientMock.Verify(
                x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), default),
                Times.Never);
        }

        [Fact]
        public void SmtpConfigOverride_DefaultsToNull()
        {
            IServiceProvider provider = this.services.BuildServiceProvider();
            var mailer = provider.GetService<ISmtpOverrideMailer>();

            Assert.NotNull(mailer);
            Assert.Null(mailer!.SmtpConfigOverride);
        }

        [Fact]
        public void SmtpConfigOverride_CanBeSetAndRead()
        {
            IServiceProvider provider = this.services.BuildServiceProvider();
            var mailer = provider.GetService<ISmtpOverrideMailer>();

            var overrideConfig = new SMTPConfigModel
            {
                Host = "custom.example.com",
                Port = 465
            };

            mailer!.SmtpConfigOverride = overrideConfig;

            Assert.Same(overrideConfig, mailer.SmtpConfigOverride);
        }
    }
}

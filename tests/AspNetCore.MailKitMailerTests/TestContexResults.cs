using Microsoft.Extensions.DependencyInjection;
using System;
using AspNetCore.MailKitMailer;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Data;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using AspNetCore.MailKitMailerTests.TestClients;
using System.Linq.Expressions;
using AspNetCore.MailKitMailerTests.TestData;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.Http;
using Xunit;
namespace AspNetCore.MailKitMailerTests
{
    public class Tests
    {
        private IServiceCollection services;

        public Tests()
        {
            this.services = new ServiceCollection();

            services.Configure<MailKitMailer.Models.MailerViewEngineOptions>(x => x = new MailKitMailer.Models.MailerViewEngineOptions());
            services.Configure<MailKitMailer.Models.SMTPConfigModel>(x => new MailKitMailer.Models.SMTPConfigModel());

            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<IMailkitSMTPClient>(x => Mock.Of<IMailkitSMTPClient>(MockBehavior.Loose));
            services.RegisterAllMailContexesOfCallingAssembly();
            services.AddScoped<ITempDataProvider>(x => Mock.Of<ITempDataProvider>(MockBehavior.Loose));

            // Http client mock
            Mock<TestClients.FakeHttpHandler> mockHandler = new Mock<TestClients.FakeHttpHandler> { CallBase = true };
            mockHandler
                .Setup(handler => handler.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage());

            var mockHttpClient = new HttpClient(mockHandler.Object);

            Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

            services.AddScoped<IHttpClientFactory>(x => mockFactory.Object);
        }

        [Theory]
        [MemberData(nameof(TestContexResultsData.GetTestCases), MemberType = typeof(TestContexResultsData))]
        public void TestContexesAreCorrect(
            string expectedView,
            Expression<Func<ITestMailer, IMailerContextResult>> expression)
        {
            string actualViewName = null;

            ViewEngineResult resultMock = null;

            // Create View Engine Mock
            Mock<IMailerViewEngine> viewEngineMock = new Mock<IMailerViewEngine>();
            viewEngineMock.Setup(x => x.FindView(It.IsAny<ActionContext>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<ActionContext, string, bool>((actionContext, viewName, mainpage) =>
                {
                    actualViewName = viewName;

                    Mock<IView> viewMock = new Mock<IView>();
                    viewMock.Setup(x => x.RenderAsync(It.IsAny<ViewContext>())).Returns(Task.CompletedTask);

                    resultMock = ViewEngineResult.Found(viewName, viewMock.Object);

                    return resultMock;
                });

            services.AddScoped<IMailerViewEngine>(x => viewEngineMock.Object);
            IServiceProvider provider = this.services.BuildServiceProvider();
            IMailClient mailClient = provider.GetService<IMailClient>();
            mailClient.Send<ITestMailer>(expression);

            Assert.Equal(expectedView, actualViewName);
        }
    }
}

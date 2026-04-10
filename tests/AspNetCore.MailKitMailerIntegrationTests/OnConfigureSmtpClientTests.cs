using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    public class OnConfigureSmtpClientTests : Abstracts.MailTestAbstracts
    {
        public OnConfigureSmtpClientTests() : base()
        {
        }

        [Fact]
        public async Task TestOnConfigureSmtpClient_Sync_SendsSuccessfully()
        {
            var response = await this.client.GetAsync("/test/client-config/sync");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Mail server should have received the email");

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);
            Assert.Equal("Test-ClientConfig", mail.Headers["Subject"]);
        }

        [Fact]
        public async Task TestOnConfigureSmtpClient_Async_SendsSuccessfully()
        {
            var response = await this.client.GetAsync("/test/client-config/async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Mail server should have received the email");

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);
            Assert.Equal("Test-ClientConfig", mail.Headers["Subject"]);
        }
    }
}

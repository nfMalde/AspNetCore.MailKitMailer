using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using netDumbster.smtp;
using Xunit;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    public class SmtpOverrideTests : Abstracts.MailTestAbstracts
    {
        public SmtpOverrideTests() : base()
        {
        }

        [Fact]
        public async Task TestSmtpOverride_SendsToOverrideServer()
        {
            // Start a second SMTP server on a different port to receive the overridden mail
            var overrideMailServer = SimpleSmtpServer.Start();
            try
            {
                var response = await this.client.GetAsync(
                    $"/test/attachment/smtp-override?host=localhost&port={overrideMailServer.Configuration.Port}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                // The override server should have received the email
                Assert.True(overrideMailServer.ReceivedEmailCount > 0, "Override SMTP server should have received the email");

                var mail = overrideMailServer.ReceivedEmail[0];
                Assert.NotNull(mail);

                // The default server should NOT have received it
                Assert.Equal(0, this.mailServer.ReceivedEmailCount);

                // Verify the from address comes from the override config
                Assert.Contains("override@example.com", mail.FromAddress.Address);
            }
            finally
            {
                overrideMailServer.Stop();
            }
        }

        [Fact]
        public async Task TestSmtpOverrideAsync_SendsToOverrideServer()
        {
            var overrideMailServer = SimpleSmtpServer.Start();
            try
            {
                var response = await this.client.GetAsync(
                    $"/test/attachment/smtp-override-async?host=localhost&port={overrideMailServer.Configuration.Port}");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                // The override server should have received the email
                Assert.True(overrideMailServer.ReceivedEmailCount > 0, "Override SMTP server should have received the email");

                var mail = overrideMailServer.ReceivedEmail[0];
                Assert.NotNull(mail);

                // The default server should NOT have received it
                Assert.Equal(0, this.mailServer.ReceivedEmailCount);

                // Verify the from address comes from the override config
                Assert.Contains("override@example.com", mail.FromAddress.Address);
            }
            finally
            {
                overrideMailServer.Stop();
            }
        }

        [Fact]
        public async Task TestWithoutOverride_SendsToDefaultServer()
        {
            // Use existing attachment endpoint which doesn't use override
            var response = await this.client.GetAsync("/test/attachment/text");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // The default server should have received it
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Default SMTP server should have received the email");
        }
    }
}

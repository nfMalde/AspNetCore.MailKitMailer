using Microsoft.AspNetCore.Http;
using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using IntegrationTestsWebApp.Models;
using System.Net;
using System.Linq;
using HtmlAgilityPack;
using AspNetCore.MailKitMailer.Models;
using AspNetCore.MailKitMailerIntegrationTests.TestData;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    public class HtmlMailSingleValueTests:Abstracts.MailTestAbstracts, IDisposable
    {
        public HtmlMailSingleValueTests() : base()
        {
            // Base contructor implements all required services.
        }
         
        [Theory]
        [ClassData(typeof(SingleValueTestData))]
        public async Task TestSingleValueSync(EmailAddressModel _to, EmailAddressModel _cc, EmailAddressModel _bcc)
        {
            SingleValuePayload payload = new SingleValuePayload()
            {
                To = _to,
                Cc = _cc,
                Bcc = _bcc 
            };

            await this._SingleValueTesting(payload, "single-sync");
        }

        [Theory]
        [ClassData(typeof(SingleValueTestData))]
        public async Task TestSingleValueAsync(EmailAddressModel _to, EmailAddressModel _cc, EmailAddressModel _bcc)
        {
            SingleValuePayload payload = new SingleValuePayload()
            {
                To = _to,
                Cc = _cc,
                Bcc = _bcc
            };

            await this._SingleValueTesting(payload, "single-async");
        }

        private async Task _SingleValueTesting(SingleValuePayload payload, string endpoint)
        {
            var response = await this.client.PostAsync("/test/html/"+ endpoint, this.MakeContent<SingleValuePayload>(payload));

            // Multiple asserts here. We need to check if everything is ok with the mail
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);
            // Mailcount is ok. Server response is ok. Diggm deeper: Lets check the content of the mail.
            var mail = this.mailServer.ReceivedEmail[0];

            // Headers ok?
            var to = mail.ToAddresses.First();
            Assert.Equal(to.Username, payload.To.Name);
            Assert.Equal(to.Address, payload.To.Email);

            if (payload.Cc != null)
            {
                string expected = $"{payload.Cc.Name} <{payload.Cc.Email}>";
                Assert.Contains(mail.Headers.AllKeys, x => x.ToLower() == "cc");
                string ccheader = mail.Headers["Cc"];

                Assert.Equal(expected, ccheader);
            }

            if (payload.Bcc != null)
            {
                /// bcc is different and not listed in the headers.
                /// But in our test server case its in the "to_addresses" field.
                /// 
                var bccaddr = mail.BccAddresses.FirstOrDefault(x => x.Address == payload.Bcc.Email);
                Assert.NotNull(bccaddr);

                Assert.Equal(payload.Bcc.Name, bccaddr.Username);
                Assert.Equal(payload.Bcc.Email, bccaddr.Address);

            }

            // Subject Ok ?
            Assert.Equal("Test_Single_Values", mail.Subject);

            // Check for htm mail:
            Assert.Contains(mail.MessageParts, x => x.HeaderData.Contains("html"));

            var htmlbody = mail.MessageParts.First(x => x.HeaderData.Contains("html"));

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlbody.BodyData);

            this.assertBody(doc, payload.To, "to");

            if (payload.Cc != null)
            {
                this.assertBody(doc, payload.Cc, "cc");
            }

            if (payload.Bcc != null)
            {
                this.assertBody(doc, payload.Bcc, "bcc");
            }
        }

       
         
    }
}

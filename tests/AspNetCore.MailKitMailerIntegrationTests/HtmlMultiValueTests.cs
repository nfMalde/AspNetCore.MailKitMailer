using AspNetCore.MailKitMailer.Models;
using AspNetCore.MailKitMailerIntegrationTests.TestData;
using HtmlAgilityPack;
using IntegrationTestsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    public class HtmlMultiValueTests: Abstracts.MailTestAbstracts
    {

        public HtmlMultiValueTests(): base()
        {

        }

        [Theory]
        [ClassData(typeof(MultiValueTestData))]
        public async Task TestMultivalueSync(IEnumerable<EmailAddressModel> to, IEnumerable<EmailAddressModel> cc, IEnumerable<EmailAddressModel> bcc)
        {
            MultiValuePayload payload = new MultiValuePayload();
           
            if (to != null)
            {
                payload.To = to.ToList();
            }

            if (cc != null)
            {
                payload.Cc = cc.ToList();
            }

            if (bcc != null)
            {
                payload.Bcc = bcc.ToList();
            }

            await this._MultiValueTesting(payload, "multi-sync");
        }

        [Theory]
        [ClassData(typeof(MultiValueTestData))]
        public async Task TestMultivalueAsync(IEnumerable<EmailAddressModel> to, IEnumerable<EmailAddressModel> cc, IEnumerable<EmailAddressModel> bcc)
        {
            MultiValuePayload payload = new MultiValuePayload();

            if (to != null)
            {
                payload.To = to.ToList();
            }

            if (cc != null)
            {
                payload.Cc = cc.ToList();
            }

            if (bcc != null)
            {
                payload.Bcc = bcc.ToList();
            }

            await this._MultiValueTesting(payload, "multi-async");
        }

        private async Task _MultiValueTesting(MultiValuePayload payload, string endpoint)
        {
            var response = await this.client.PostAsync("/test/html/" + endpoint, this.MakeContent<MultiValuePayload>(payload));

            // Multiple asserts here. We need to check if everything is ok with the mail
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);
            // Mailcount is ok. Server response is ok. Diggm deeper: Lets check the content of the mail.
            var mail = this.mailServer.ReceivedEmail[0];
            var merge = payload.To.Concat(payload.Cc).Concat(payload.Bcc);
            foreach (var to in mail.ToAddresses)
            {
                // Ensure all to addresses are found
                Assert.Contains(merge, x => x.Email == to.Address && x.Name == to.Username);
            }

            if (payload.Cc != null)
            {
                // Ensure cc headers are correct

                foreach(var cc in payload.Cc)
                {
                    string expected = $"{cc.Name} <{cc.Email}>";
                    Assert.Contains(mail.Headers.AllKeys, x => x.ToLower() == "cc");
                    string ccheader = mail.Headers["Cc"];
                    var ccHeaderarray = ccheader.Split(',').Select(x => x.Trim().Replace('\t', ' '));

                    Assert.Contains(expected, ccHeaderarray);
                }
            }

            

            // Subject Ok ?
            Assert.Equal("Test_Multiple_Values", mail.Subject);

            // Check for htm mail:
            Assert.Contains(mail.MessageParts, x => x.HeaderData.Contains("html"));

            var htmlbody = mail.MessageParts.First(x => x.HeaderData.Contains("html"));

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlbody.BodyData);

             
            foreach(var t in payload.To)
            {
                this.assertBody(doc, t, "to");
            }
            

            if (payload.Cc != null)
            {
                foreach(var cc in payload.Cc)
                {
                    this.assertBody(doc, cc, "cc");
                }
            }

            if (payload.Bcc != null)
            {
                foreach (var bcc in payload.Bcc)
                {
                    this.assertBody(doc, bcc, "bcc");
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
namespace AspNetCore.MailKitMailerIntegrationTests
{
    public class AttachmentTests:Abstracts.MailTestAbstracts
    {
        public AttachmentTests():base()
        {

        }



        [Fact]
        public async Task TestAttachment()
        {
            var response = await this.client.GetAsync("/test/attachment/text");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail);
            Assert.Contains(mail.MessageParts, x => x.HeaderData == "text/plain; name=TestFile.txt");
            var attachment = mail.MessageParts.ToList().FirstOrDefault(x => x.HeaderData == "text/plain; name=TestFile.txt");

            Assert.Equal("This is a test file for testing attachments.", attachment.BodyData);
            
        }
        
        [Fact]
        public async Task TestAttachmentDownload()
        {
            var dlserver = this.StartDownloadServer();

            var response = await this.client.GetAsync("/test/attachment/test-download");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail);
            Assert.Contains(mail.MessageParts, x => x.HeaderData == "text/plain; name=TestFile.txt");
            var attachment = mail.MessageParts.ToList().FirstOrDefault(x => x.HeaderData == "text/plain; name=TestFile.txt");

            Assert.Equal("TestDownload", attachment.BodyData);

            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestAttachmentDownloadWithoutFilename()
        {
            var dlserver = this.StartDownloadServer();

            var response = await this.client.GetAsync("/test/attachment/test-download2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail);
            Assert.Contains(mail.MessageParts, x => x.HeaderData == "text/plain; name=NoName.txt");
            var attachment = mail.MessageParts.ToList().FirstOrDefault(x => x.HeaderData == "text/plain; name=NoName.txt");

            Assert.Equal("TestDownload2", attachment.BodyData);


            await this.StopDownloadServer();
        }
    }
}

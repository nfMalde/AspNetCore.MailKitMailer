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
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=TestFile.txt"));

            Assert.NotNull(attachment);
 
            Assert.Equal("This is a test file for testing attachments.", attachment.BodyData);
            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestAttachmentAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/text-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail);
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=TestFile.txt"));

            Assert.NotNull(attachment);
 
            Assert.Equal("This is a test file for testing attachments.", attachment.BodyData);
            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestAttachmentBytes()
        {
            var response = await this.client.GetAsync("/test/attachment/text-bytes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail);
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=fixedname.txt"));

            Assert.NotNull(attachment);

            Assert.Equal("This is a test file for testing attachments.", attachment.BodyData);
            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestAttachmentBytesAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/text-bytes-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail);
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=fixedname.txt"));

            Assert.NotNull(attachment);

            Assert.Equal("This is a test file for testing attachments.", attachment.BodyData);
            await this.StopDownloadServer();
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
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=TestFile.txt"));

            Assert.NotNull(attachment);
            Assert.Equal("TestDownload", attachment.BodyData);

            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestAttachmentDownloadAsync()
        {
            var dlserver = this.StartDownloadServer();

            var response = await this.client.GetAsync("/test/attachment/test-download-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail); 
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=TestFile.txt"));

            Assert.NotNull(attachment);
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
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=NoName.txt"));

            Assert.NotNull(attachment);
            Assert.Equal("TestDownload2", attachment.BodyData);

            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestAttachmentDownloadWithoutFilenameAsync()
        {
            var dlserver = this.StartDownloadServer();

            var response = await this.client.GetAsync("/test/attachment/test-download2-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];

            Assert.NotNull(mail); 
            var attachment = mail.MessageParts.FirstOrDefault(x => x.HeaderData.Contains("name=NoName.txt"));

            Assert.NotNull(attachment);
            Assert.Equal("TestDownload2", attachment.BodyData);

            await this.StopDownloadServer();
        }
    }
}

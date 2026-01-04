using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    /// <summary>
    /// Integration tests for CID (Content-ID) linked resources support
    /// </summary>
    public class LinkedResourceTests : Abstracts.MailTestAbstracts
    {
        public LinkedResourceTests() : base()
        {
        }

        [Fact]
        public async Task TestLinkedResourceFile()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-file");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the linked resource is present with correct Content-ID
            var linkedResource = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("Content-ID: <testimage>") || 
                x.HeaderData.Contains("Content-Id: <testimage>"));

            Assert.NotNull(linkedResource);
            Assert.Contains("inline", linkedResource.HeaderData.ToLower());
        }

        [Fact]
        public async Task TestLinkedResourceFileAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-file-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the linked resource is present with correct Content-ID
            var linkedResource = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("Content-ID: <testimage>") || 
                x.HeaderData.Contains("Content-Id: <testimage>"));

            Assert.NotNull(linkedResource);
            Assert.Contains("inline", linkedResource.HeaderData.ToLower());
        }

        [Fact]
        public async Task TestLinkedResourceBytes()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-bytes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the linked resource is present with correct Content-ID
            var linkedResource = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("Content-ID: <testimage>") || 
                x.HeaderData.Contains("Content-Id: <testimage>"));

            Assert.NotNull(linkedResource);
            Assert.Contains("inline", linkedResource.HeaderData.ToLower());
        }

        [Fact]
        public async Task TestLinkedResourceBytesAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-bytes-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the linked resource is present with correct Content-ID
            var linkedResource = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("Content-ID: <testimage>") || 
                x.HeaderData.Contains("Content-Id: <testimage>"));

            Assert.NotNull(linkedResource);
            Assert.Contains("inline", linkedResource.HeaderData.ToLower());
        }

        [Fact]
        public async Task TestLinkedResourceUrl()
        {
            this.StartDownloadServer();

            var response = await this.client.GetAsync("/test/attachment/linked-resource-url");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the linked resource is present with correct Content-ID
            var linkedResource = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("Content-ID: <testimage>") || 
                x.HeaderData.Contains("Content-Id: <testimage>"));

            Assert.NotNull(linkedResource);
            Assert.Contains("inline", linkedResource.HeaderData.ToLower());

            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestLinkedResourceUrlAsync()
        {
            this.StartDownloadServer();

            var response = await this.client.GetAsync("/test/attachment/linked-resource-url-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the linked resource is present with correct Content-ID
            var linkedResource = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("Content-ID: <testimage>") || 
                x.HeaderData.Contains("Content-Id: <testimage>"));

            Assert.NotNull(linkedResource);
            Assert.Contains("inline", linkedResource.HeaderData.ToLower());

            await this.StopDownloadServer();
        }

        [Fact]
        public async Task TestLinkedResourceHtmlContainsCidReference()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-file");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0);

            var mail = this.mailServer.ReceivedEmail[0];
            Assert.NotNull(mail);

            // Verify the HTML body contains the cid: reference
            var htmlPart = mail.MessageParts.FirstOrDefault(x => 
                x.HeaderData.Contains("text/html"));

            Assert.NotNull(htmlPart);
            Assert.Contains("cid:testimage", htmlPart.BodyData);
        }
    }
}

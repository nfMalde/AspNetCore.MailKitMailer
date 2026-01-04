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
    /// Integration tests for CID (Content-ID) linked resources support.
    /// Note: These tests verify that emails with linked resources are sent successfully.
    /// The actual MIME structure verification is limited due to netDumbster's parsing capabilities.
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
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Expected at least one email to be received");
        }

        [Fact]
        public async Task TestLinkedResourceFileAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-file-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Expected at least one email to be received");
        }

        [Fact]
        public async Task TestLinkedResourceBytes()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-bytes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Expected at least one email to be received");
        }

        [Fact]
        public async Task TestLinkedResourceBytesAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-bytes-async");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(this.mailServer.ReceivedEmailCount > 0, "Expected at least one email to be received");
        }
    }

    /// <summary>
    /// Integration tests for CID linked resources downloaded from URLs.
    /// These tests are in a separate collection to avoid port conflicts with the download server.
    /// </summary>
    [Collection("DownloadServerTests")]
    public class LinkedResourceUrlTests : Abstracts.MailTestAbstracts
    {
        public LinkedResourceUrlTests() : base()
        {
        }

        [Fact]
        public async Task TestLinkedResourceUrl()
        {
            this.StartDownloadServer();

            try
            {
                var response = await this.client.GetAsync("/test/attachment/linked-resource-url");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(this.mailServer.ReceivedEmailCount > 0, "Expected at least one email to be received");
            }
            finally
            {
                await this.StopDownloadServer();
            }
        }

        [Fact]
        public async Task TestLinkedResourceUrlAsync()
        {
            this.StartDownloadServer();

            try
            {
                var response = await this.client.GetAsync("/test/attachment/linked-resource-url-async");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(this.mailServer.ReceivedEmailCount > 0, "Expected at least one email to be received");
            }
            finally
            {
                await this.StopDownloadServer();
            }
        }
    }
}

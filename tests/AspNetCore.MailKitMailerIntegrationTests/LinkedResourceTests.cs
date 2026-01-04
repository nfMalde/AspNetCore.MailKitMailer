using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    /// <summary>
    /// Integration tests for CID (Content-ID) linked resources support.
    /// Note: These tests verify that emails with linked resources are sent successfully
    /// by checking the raw MIME data, as netDumbster's MessageParts parser doesn't 
    /// correctly handle nested multipart structures (multipart/related inside multipart/alternative).
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
            
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            this.mailServer.ReceivedEmailCount.ShouldBeGreaterThan(0, "Expected at least one email to be received");

            var email = this.mailServer.ReceivedEmail.FirstOrDefault();
            email.ShouldNotBeNull();
            
            // Check raw email data for proper MIME structure
            var rawData = email.Data;
            
            // Verify the Content-ID header is present with our custom ID
            rawData.ShouldContain("Content-Id: <testimage>", customMessage: 
                "Expected raw email to contain Content-Id header with 'testimage'");
            
            // Verify the HTML body references the CID
            rawData.ShouldContain("cid:testimage", customMessage: 
                "Expected HTML body to contain CID reference");
            
            // Verify multipart/related structure is present
            rawData.ShouldContain("multipart/related", customMessage: 
                "Expected email to have multipart/related structure for linked resources");
        }

        [Fact]
        public async Task TestLinkedResourceFileAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-file-async");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            this.mailServer.ReceivedEmailCount.ShouldBeGreaterThan(0, "Expected at least one email to be received");

            var email = this.mailServer.ReceivedEmail.FirstOrDefault();
            email.ShouldNotBeNull();
            
            // Check raw email data for proper MIME structure
            var rawData = email.Data;
            
            // Verify the Content-ID header is present with our custom ID
            rawData.ShouldContain("Content-Id: <testimage>", customMessage: 
                "Expected raw email to contain Content-Id header with 'testimage'");
            
            // Verify multipart/related structure is present
            rawData.ShouldContain("multipart/related", customMessage: 
                "Expected email to have multipart/related structure for linked resources");
        }

        [Fact]
        public async Task TestLinkedResourceBytes()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-bytes");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            this.mailServer.ReceivedEmailCount.ShouldBeGreaterThan(0, "Expected at least one email to be received");

            var email = this.mailServer.ReceivedEmail.FirstOrDefault();
            email.ShouldNotBeNull();
            
            // Check raw email data for proper MIME structure
            var rawData = email.Data;
            
            // Verify the Content-ID header is present with our custom ID
            rawData.ShouldContain("Content-Id: <testimage>", customMessage: 
                "Expected raw email to contain Content-Id header with 'testimage'");
            
            // Verify multipart/related structure is present
            rawData.ShouldContain("multipart/related", customMessage: 
                "Expected email to have multipart/related structure for linked resources");
        }

        [Fact]
        public async Task TestLinkedResourceBytesAsync()
        {
            var response = await this.client.GetAsync("/test/attachment/linked-resource-bytes-async");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            this.mailServer.ReceivedEmailCount.ShouldBeGreaterThan(0, "Expected at least one email to be received");

            var email = this.mailServer.ReceivedEmail.FirstOrDefault();
            email.ShouldNotBeNull();
            
            // Check raw email data for proper MIME structure
            var rawData = email.Data;
            
            // Verify the Content-ID header is present with our custom ID
            rawData.ShouldContain("Content-Id: <testimage>", customMessage: 
                "Expected raw email to contain Content-Id header with 'testimage'");
            
            // Verify multipart/related structure is present
            rawData.ShouldContain("multipart/related", customMessage: 
                "Expected email to have multipart/related structure for linked resources");
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

                response.StatusCode.ShouldBe(HttpStatusCode.OK);
                this.mailServer.ReceivedEmailCount.ShouldBeGreaterThan(0, "Expected at least one email to be received");

                var email = this.mailServer.ReceivedEmail.FirstOrDefault();
                email.ShouldNotBeNull();
                
                // Check raw email data for proper MIME structure
                var rawData = email.Data;
                
                // Verify the Content-ID header is present with our custom ID
                rawData.ShouldContain("Content-Id: <testimage>", customMessage: 
                    "Expected raw email to contain Content-Id header with 'testimage'");
                
                // Verify multipart/related structure is present
                rawData.ShouldContain("multipart/related", customMessage: 
                    "Expected email to have multipart/related structure for linked resources");
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

                response.StatusCode.ShouldBe(HttpStatusCode.OK);
                this.mailServer.ReceivedEmailCount.ShouldBeGreaterThan(0, "Expected at least one email to be received");

                var email = this.mailServer.ReceivedEmail.FirstOrDefault();
                email.ShouldNotBeNull();
                
                // Check raw email data for proper MIME structure
                var rawData = email.Data;
                
                // Verify the Content-ID header is present with our custom ID
                rawData.ShouldContain("Content-Id: <testimage>", customMessage: 
                    "Expected raw email to contain Content-Id header with 'testimage'");
                
                // Verify multipart/related structure is present
                rawData.ShouldContain("multipart/related", customMessage: 
                    "Expected email to have multipart/related structure for linked resources");
            }
            finally
            {
                await this.StopDownloadServer();
            }
        }
    }
}

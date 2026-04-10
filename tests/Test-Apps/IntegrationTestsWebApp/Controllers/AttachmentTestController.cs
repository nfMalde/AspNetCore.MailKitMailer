using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using IntegrationTestsWebApp.Mailer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Controllers
{
    [Route("test/attachment")]
    public class AttachmentTestController : Controller
    {
        private readonly IMailClient client;
        private readonly IWebHostEnvironment webHost;
        private readonly HttpClient httpClient;

        public AttachmentTestController(
            IMailClient client, 
            IWebHostEnvironment webHost,
            IHttpClientFactory httpClientFactory)
        {
            this.client = client;
            this.webHost = webHost;
            this.httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("text")]
        public IActionResult TestTextFile()
        {
            string myfile = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestFile.txt");

            this.client.Send<ITestMailer>(x =>
                x.Test_Attachment(myfile)
            );

            return Ok();
        }

        [HttpGet("text-async")]
        public async Task<IActionResult> TestTextFileAsync()
        {
            string myfile = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestFile.txt");

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_AttachmentAsync(myfile)
            );

            return Ok();
        }

        [HttpGet("text-bytes")]
        public IActionResult TestTextFileBytes()
        {
            string myfile = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestFile.txt");

            this.client.Send<ITestMailer>(x =>
                x.Test_AttachmentBytes(myfile)
            );

            return Ok();
        }

        [HttpGet("text-bytes-async")]
        public async Task<IActionResult> TestTextFileBytesAsync()
        {
            string myfile = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestFile.txt");

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_AttachmentBytesAsync(myfile)
            );

            return Ok();
        }

        [HttpGet("test-download")]
        public IActionResult TestDownload()
        {
            string testuri = "http://localhost:3333/dl/TestFile.txt";
           
            Uri downloadUri = new Uri(testuri);

            this.client.Send<ITestMailer>(x =>
                x.Test_Attachment_Download(downloadUri)
            );

            return Ok();
        }

        [HttpGet("test-download-async")]
        public async Task<IActionResult> TestDownloadAsync()
        {
            string testuri = "http://localhost:3333/dl/TestFile.txt";
           
            Uri downloadUri = new Uri(testuri);

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_Attachment_DownloadAsync(downloadUri)
            );

            return Ok();
        }

        [HttpGet("test-download2")]
        public IActionResult TestDownload2()
        {
            string testuri = "http://localhost:3333/dl2/NoName";

            Uri downloadUri = new Uri(testuri);

            this.client.Send<ITestMailer>(x =>
                x.Test_Attachment_Download(downloadUri)
            );

            return Ok();
        }

        [HttpGet("test-download2-async")]
        public async Task<IActionResult> TestDownload2Async()
        {
            string testuri = "http://localhost:3333/dl2/NoName";

            Uri downloadUri = new Uri(testuri);

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_Attachment_DownloadAsync(downloadUri)
            );

            return Ok();
        }

        [HttpGet("linked-resource-file")]
        public IActionResult TestLinkedResourceFile()
        {
            string imagePath = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestImage.png");

            this.client.Send<ITestMailer>(x =>
                x.Test_LinkedResource_File(imagePath, "testimage")
            );

            return Ok();
        }

        [HttpGet("linked-resource-file-async")]
        public async Task<IActionResult> TestLinkedResourceFileAsync()
        {
            string imagePath = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestImage.png");

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_LinkedResource_FileAsync(imagePath, "testimage")
            );

            return Ok();
        }

        [HttpGet("linked-resource-bytes")]
        public IActionResult TestLinkedResourceBytes()
        {
            string imagePath = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestImage.png");
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

            this.client.Send<ITestMailer>(x =>
                x.Test_LinkedResource_Bytes(imageBytes, "TestImage.png", "image/png", "testimage")
            );

            return Ok();
        }

        [HttpGet("linked-resource-bytes-async")]
        public async Task<IActionResult> TestLinkedResourceBytesAsync()
        {
            string imagePath = Path.Combine(this.webHost.ContentRootPath, "TestData", "TestImage.png");
            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_LinkedResource_BytesAsync(imageBytes, "TestImage.png", "image/png", "testimage")
            );

            return Ok();
        }

        [HttpGet("linked-resource-url")]
        public IActionResult TestLinkedResourceUrl()
        {
            string testuri = "http://localhost:3333/dl/TestImage.png";
            Uri downloadUri = new Uri(testuri);

            this.client.Send<ITestMailer>(x =>
                x.Test_LinkedResource_Url(downloadUri, "testimage")
            );

            return Ok();
        }

        [HttpGet("linked-resource-url-async")]
        public async Task<IActionResult> TestLinkedResourceUrlAsync()
        {
            string testuri = "http://localhost:3333/dl/TestImage.png";
            Uri downloadUri = new Uri(testuri);

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_LinkedResource_UrlAsync(downloadUri, "testimage")
            );

            return Ok();
        }

        [HttpGet("smtp-override")]
        public IActionResult TestSmtpOverride([FromQuery] string host, [FromQuery] int port)
        {
            var overrideConfig = new SMTPConfigModel
            {
                Host = host,
                Port = port,
                UseSSL = false,
                DoAuthenticate = false,
                CheckCertificateRevocation = false,
                FromAddress = new EmailAddressModel("Override Sender", "override@example.com")
            };

            this.client.Send<ITestMailer>(x =>
                x.Test_SmtpOverride(overrideConfig)
            );

            return Ok();
        }

        [HttpGet("smtp-override-async")]
        public async Task<IActionResult> TestSmtpOverrideAsync([FromQuery] string host, [FromQuery] int port)
        {
            var overrideConfig = new SMTPConfigModel
            {
                Host = host,
                Port = port,
                UseSSL = false,
                DoAuthenticate = false,
                CheckCertificateRevocation = false,
                FromAddress = new EmailAddressModel("Override Sender", "override@example.com")
            };

            await this.client.SendAsync<ITestMailer>(x =>
                x.Test_SmtpOverrideAsync(overrideConfig)
            );

            return Ok();
        }
    }
}

using AspNetCore.MailKitMailer.Domain;
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
    }
}

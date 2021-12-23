using AspNetCore.MailKitMailer.Domain;
using IntegrationTestsWebApp.Mailer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Controllers
{
    [Route("test/attachment")]
    public class AttachmentTestController : Controller
    {
        private readonly IMailClient client;
        private readonly IWebHostEnvironment webHost;

        public AttachmentTestController(IMailClient client, IWebHostEnvironment webHost)
        {
            this.client = client;
            this.webHost = webHost;
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
    }
}

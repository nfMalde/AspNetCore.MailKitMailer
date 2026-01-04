using AspNetCore.MailKitMailer.Domain;
using MailKitMailerExample.Mailer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MailKitMailerExample.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IMailClient client;
        private readonly IWebHostEnvironment webHost;

        public TestController(IMailClient client, IWebHostEnvironment webHost)
        {
            this.client = client;
            this.webHost = webHost;
        }

        [HttpGet("welcome")]
        public IActionResult Welcome()
        {
            string username = "John.Doe";
            string useremail = "john@example.com";

            this.client.Send<ITestMailer>(x => x.WelcomeMail(username, useremail));
            
            return View();
        }

        [HttpGet("welcome-2")]
        public async Task<IActionResult> WelcomeMultipleTos()
        {
            // this is an example for an welcome mail sending to multiple users
            // For this example we first need to create some kind of list that holds the users
            Dictionary<string, string> users = new Dictionary<string, string>();
            users.Add("john@example.com", "John");
            users.Add("jonny@example.com", "Jonny");

            // As you can see we added an method "WelcomeMailMultiple"
            // for this into our mailing contex: which accepts the users list as parameter
            await this.client.SendAsync<ITestMailer>(x => x.WelcomeMailMultiple(users));

            return View("Welcome");
        }

        [HttpGet("welcome-with-logo")]
        public IActionResult WelcomeWithLogo()
        {
            // Example: Sending a welcome email with an embedded inline logo image using CID
            string username = "John.Doe";
            string useremail = "john@example.com";
            
            // Placeholder path to a logo image. This assumes a file at wwwroot/images/logo.png;
            // add that file to your project or change the path below to point to an existing image.
            string logoPath = Path.Combine(this.webHost.WebRootPath, "images", "logo.png");

            // The logo will be embedded in the email using CID (Content-ID)
            // In the email template, it's referenced as: <img src="cid:company-logo" />
            this.client.Send<ITestMailer>(x => x.WelcomeMailWithLogo(username, useremail, logoPath));
            
            return View("Welcome");
        }
    }
}

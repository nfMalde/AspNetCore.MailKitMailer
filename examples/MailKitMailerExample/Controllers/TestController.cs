using AspNetCore.MailKitMailer.Domain;
using MailKitMailerExample.Mailer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailKitMailerExample.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IMailClient client;

        public TestController(IMailClient client)
        {
            this.client = client;
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
    }
}

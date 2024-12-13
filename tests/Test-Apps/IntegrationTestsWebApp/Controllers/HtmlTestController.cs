using AspNetCore.MailKitMailer.Domain;
using IntegrationTestsWebApp.Mailer;
using IntegrationTestsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Controllers
{
    [Route("test/html")]
    public class HtmlTestController : Controller
    {
        private readonly IMailClient client;

        public HtmlTestController(IMailClient client)
        {
            this.client = client;
        }

        [HttpPost("single-sync")]
        public IActionResult SendWithSingleValueSync([FromBody] SingleValuePayload payload)
        {
            client.GetContentAsync<ITestMailer>(x => x.Test_Single_Values(payload));
            this.client.Send<ITestMailer>(x => x.Test_Single_Values(payload));

            return Ok();
        }

        [HttpPost("single-async")]
        public async Task<IActionResult> SendWithSingleValueAsync([FromBody] SingleValuePayload payload)
        {

            await this.client.SendAsync<ITestMailer>(x => x.Test_Single_Values(payload));

            return Ok();
        }

        [HttpPost("multi-sync")]
        public IActionResult SendWithMultiValuesSync([FromBody] MultiValuePayload payload)
        {
            
            this.client.Send<ITestMailer>(x => x.Test_Multiple_Values(payload));

            return Ok();
        }

        [HttpPost("multi-async")]
        public async Task<IActionResult> SendWithMultiValuesAsync([FromBody] MultiValuePayload payload)
        {

            await this.client.SendAsync<ITestMailer>(x => x.Test_Multiple_Values(payload));

            return Ok();
        }
    }
}

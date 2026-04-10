using AspNetCore.MailKitMailer.Domain;
using IntegrationTestsWebApp.Mailer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Controllers
{
    [Route("test/client-config")]
    public class ClientConfigTestController : Controller
    {
        private readonly IMailClient client;

        public ClientConfigTestController(IMailClient client)
        {
            this.client = client;
        }

        [HttpGet("sync")]
        public IActionResult TestSync()
        {
            this.client.Send<IClientConfigTestMailer>(x =>
                x.Test_WithClientConfig()
            );

            return Ok();
        }

        [HttpGet("async")]
        public async Task<IActionResult> TestAsync()
        {
            await this.client.SendAsync<IClientConfigTestMailer>(x =>
                x.Test_WithClientConfigAsync()
            );

            return Ok();
        }
    }
}

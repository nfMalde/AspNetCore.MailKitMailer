using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Mailer
{
    public interface IClientConfigTestMailer : IMailerContext
    {
        IMailerContextResult Test_WithClientConfig();
        Task<IMailerContextResult> Test_WithClientConfigAsync();
    }

    public class ClientConfigTestMailer : AspNetCore.MailKitMailer.Data.MailerContextAbstract, IClientConfigTestMailer
    {
        public override void OnConfigureSmtpClient(SmtpClient client)
        {
            // Remove XOAUTH2 as a demonstration of client configuration
            client.AuthenticationMechanisms.Remove("XOAUTH2");
        }

        public IMailerContextResult Test_WithClientConfig()
        {
            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-ClientConfig");
        }

        public async Task<IMailerContextResult> Test_WithClientConfigAsync()
        {
            await Task.Delay(1);
            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-ClientConfig");
        }
    }
}

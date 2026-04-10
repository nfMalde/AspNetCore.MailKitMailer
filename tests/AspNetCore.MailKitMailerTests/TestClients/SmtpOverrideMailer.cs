using System;
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;

namespace AspNetCore.MailKitMailerTests.TestClients
{
    public interface ISmtpOverrideMailer : IMailerContext
    {
        IMailerContextResult Html_WithOverride();
        IMailerContextResult Html_WithoutOverride();
    }

    public class SmtpOverrideMailer : MailerContextAbstract, ISmtpOverrideMailer
    {
        public IMailerContextResult Html_WithOverride()
        {
            this.SmtpConfigOverride = new SMTPConfigModel
            {
                Host = "override-smtp.example.com",
                Port = 587,
                UseSSL = true,
                DoAuthenticate = true,
                Username = "overrideuser",
                Password = "overridepass",
                CheckCertificateRevocation = true,
                FromAddress = new EmailAddressModel("Override Sender", "override@example.com")
            };

            return HtmlMail(
                new EmailAddressModel("John", "john@localhost"),
                "Override Test");
        }

        public IMailerContextResult Html_WithoutOverride()
        {
            return HtmlMail(
                new EmailAddressModel("John", "john@localhost"),
                "No Override Test");
        }
    }
}

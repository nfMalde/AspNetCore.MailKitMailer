using System;
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using MailKit.Net.Smtp;

namespace AspNetCore.MailKitMailerTests.TestClients
{
    public interface IClientConfigMailer : IMailerContext
    {
        IMailerContextResult Html_WithClientConfig();
        IMailerContextResult Html_WithoutClientConfig();
    }

    public class ClientConfigMailer : MailerContextAbstract, IClientConfigMailer
    {
#nullable enable
        public bool ConfigureClientCalled { get; private set; }
        public SmtpClient? ConfiguredClient { get; private set; }
#nullable restore

        public override void OnConfigureSmtpClient(SmtpClient client)
        {
            ConfigureClientCalled = true;
            ConfiguredClient = client;
            client.AuthenticationMechanisms.Remove("XOAUTH2");
        }

        public IMailerContextResult Html_WithClientConfig()
        {
            return HtmlMail(
                new EmailAddressModel("John", "john@localhost"),
                "Client Config Test");
        }

        public IMailerContextResult Html_WithoutClientConfig()
        {
            return HtmlMail(
                new EmailAddressModel("John", "john@localhost"),
                "No Client Config Test");
        }
    }
}

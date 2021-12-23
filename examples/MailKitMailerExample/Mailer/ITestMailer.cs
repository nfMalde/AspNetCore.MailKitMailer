using AspNetCore.MailKitMailer.Domain;
using System.Collections.Generic;

namespace MailKitMailerExample.Mailer
{
    public interface ITestMailer:IMailerContext
    {
        IMailerContextResult WelcomeMail(string username, string email);
        IMailerContextResult WelcomeMailMultiple(Dictionary<string, string> users);
    }
}
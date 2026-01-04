using AspNetCore.MailKitMailer.Domain;
using System.Collections.Generic;

namespace MailKitMailerExample.Mailer
{
    public interface ITestMailer:IMailerContext
    {
        IMailerContextResult WelcomeMail(string username, string email);
        IMailerContextResult WelcomeMailMultiple(Dictionary<string, string> users);
        
        /// <summary>
        /// Sends a welcome email with an inline logo image using CID
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="email">The email address</param>
        /// <param name="logoPath">Path to the logo image file</param>
        IMailerContextResult WelcomeMailWithLogo(string username, string email, string logoPath);
    }
}
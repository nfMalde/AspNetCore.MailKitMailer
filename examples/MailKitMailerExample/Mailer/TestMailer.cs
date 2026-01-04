using System;
using System.Collections.Generic;
using System.Linq; 
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using MailKitMailerExample.Models.MailModels;

namespace MailKitMailerExample.Mailer
{
    public class TestMailer : MailerContextAbstract, ITestMailer
    {
        public TestMailer()
        {
            this.DefaultReceipients.Add(new EmailAddressModel("admin", "admin@localhost"));
        }


        public IMailerContextResult WelcomeMail(string username, string email)
        {
            return this.HtmlMail(new EmailAddressModel(username, email),
                $"Welcome {username}!",

                new WelcomeModel() { Username = username, Date = DateTime.Now });
        }

        public IMailerContextResult WelcomeMailMultiple(Dictionary<string,string> users)
        {
            // Create our view model
            WelcomeModelMultiple welcomeModelMultiple = new WelcomeModelMultiple();
            // Assigning the usernames in this case the values of the dictionary are the usernames and the keys are the email addresses
            welcomeModelMultiple.Usernames.AddRange(users.Values);
            // Create our email address models for the contex
            List<EmailAddressModel> emailAddresses = 
                // Name=Value, Address = Key
                users.Select(x => new EmailAddressModel(x.Value, x.Key)).ToList();

            // Return
            return HtmlMail(emailAddresses, "Welcome dudes!", welcomeModelMultiple);

        }

        /// <summary>
        /// Sends a welcome email with an inline logo image using CID (Content-ID).
        /// The logo will be embedded in the email and can be referenced in HTML via cid:company-logo
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="email">The email address</param>
        /// <param name="logoPath">Path to the logo image file</param>
        public IMailerContextResult WelcomeMailWithLogo(string username, string email, string logoPath)
        {
            // Add the logo as a linked resource with Content-ID "company-logo"
            // This can be referenced in the view using: <img src="cid:company-logo" />
            return this.HtmlMail(
                new EmailAddressModel(username, email),
                $"Welcome {username}!",
                new WelcomeModel() { Username = username, Date = DateTime.Now },
                withAttachments: a => a.AddLinkedResource(logoPath, "company-logo"));
        }
    }
}

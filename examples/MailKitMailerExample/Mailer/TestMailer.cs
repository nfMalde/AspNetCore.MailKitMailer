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
    }
}

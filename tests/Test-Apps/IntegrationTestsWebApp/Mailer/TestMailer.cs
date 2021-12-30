using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using IntegrationTestsWebApp.Models;
using IntegrationTestsWebApp.Models.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Mailer
{
    public interface ITestMailer:IMailerContext
    {
        IMailerContextResult Test_Attachment_Download(Uri attachmentUri);
        IMailerContextResult Test_Single_Values(SingleValuePayload payload);
        IMailerContextResult Test_Multiple_Values(Models.MultiValuePayload payload);
        IMailerContextResult Test_Attachment(string attachmentPath);
    }

    public class TestMailer : AspNetCore.MailKitMailer.Data.MailerContextAbstract, ITestMailer
    {
        public TestMailer()
        {
        }


        public IMailerContextResult Test_Attachment(string attachmentPath)
        {


            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-Attachment",
                null,
                null,
                x => x.Add(attachmentPath)
                );
        }

        public IMailerContextResult Test_Attachment_Download(Uri attachmentUri)
        {


            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-Attachment Download",
                null,
                null,
                x => x.Add(attachmentUri)
                );
        }

        public IMailerContextResult Test_Single_Values(Models.SingleValuePayload payload)
        {


            return HtmlMail(
                payload.To, 
                payload.Cc, 
                payload.Bcc, 
                "Test_Single_Values", 
                new SingleValueModel()
                {
                    Data = payload
                }
                );
        }

        public IMailerContextResult Test_Multiple_Values(Models.MultiValuePayload payload)
        {
            return HtmlMail(
                payload.To, 
                payload.Cc, 
                payload.Bcc, 
                "Test_Multiple_Values", 
                new MultiValueModel() { 
                    Data = payload
                });
        }
    }
}

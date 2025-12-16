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
        Task<IMailerContextResult> Test_Attachment_DownloadAsync(Uri attachmentUri);
        
        IMailerContextResult Test_Single_Values(SingleValuePayload payload);
        Task<IMailerContextResult> Test_Single_ValuesAsync(SingleValuePayload payload);
        
        IMailerContextResult Test_Multiple_Values(Models.MultiValuePayload payload);
        Task<IMailerContextResult> Test_Multiple_ValuesAsync(Models.MultiValuePayload payload);
        
        IMailerContextResult Test_Attachment(string attachmentPath);
        Task<IMailerContextResult> Test_AttachmentAsync(string attachmentPath);

        IMailerContextResult Test_AttachmentBytes(string attachmentPath);
        Task<IMailerContextResult> Test_AttachmentBytesAsync(string attachmentPath);
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

        public async Task<IMailerContextResult> Test_AttachmentAsync(string attachmentPath)
        {
            await Task.Delay(1);
            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-Attachment",
                null,
                null,
                x => x.Add(attachmentPath)
                );
        }

        public IMailerContextResult Test_AttachmentBytes(string attachmentPath)
        {
            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-Attachment",
                null,
                null,
                x => x.Add(System.IO.File.ReadAllBytes(attachmentPath), "fixedname.txt", "text/plain")
                );
        }

        public async Task<IMailerContextResult> Test_AttachmentBytesAsync(string attachmentPath)
        {
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(attachmentPath);
            return HtmlMail(
                new EmailAddressModel("test", "test@localhost"),
                "Test-Attachment",
                null,
                null,
                x => x.Add(fileBytes, "fixedname.txt", "text/plain")
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

        public async Task<IMailerContextResult> Test_Attachment_DownloadAsync(Uri attachmentUri)
        {
            await Task.Delay(1);
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

        public async Task<IMailerContextResult> Test_Single_ValuesAsync(Models.SingleValuePayload payload)
        {
            await Task.Delay(1);
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

        public async Task<IMailerContextResult> Test_Multiple_ValuesAsync(Models.MultiValuePayload payload)
        {
            await Task.Delay(1);
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

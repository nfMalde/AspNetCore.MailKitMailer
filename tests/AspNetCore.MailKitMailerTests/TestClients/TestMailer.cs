using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;


namespace AspNetCore.MailKitMailerTests.TestClients
{
    public interface ITestMailer: IMailerContext
    {
        IMailerContextResult Html_01_FixedViewName();
        IMailerContextResult Html_01_GeneratedViewName();
        IMailerContextResult Text_01();
    }

    public class TestMailer : MailerContextAbstract, ITestMailer
    {
        public TestMailer()
        {

        }



        public IMailerContextResult Html_01_GeneratedViewName()
        {
            return HtmlMail(new MailKitMailer.Models.EmailAddressModel("John", "John@localhost"), "Html_01_GeneratedViewName");
        }

        public IMailerContextResult Html_01_FixedViewName()
        {
            return HtmlMail(
                new MailKitMailer.Models.EmailAddressModel("John", "John@localhost"),
                "Html_01_GeneratedViewName",
                viewName: "fixedView");
        }

        public IMailerContextResult Text_01()
        {
            return PlainTextMail(
                new MailKitMailer.Models.EmailAddressModel("John", "John@localhost"),
                "Text_01",
                "Test_01"
                );
        }

    }
}

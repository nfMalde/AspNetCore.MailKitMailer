using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailerTests.TestClients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspNetCore.MailKitMailerTests.TestData
{
    public class TestContexResultsData : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { "TestMailer/Html_01_GeneratedViewName",  this.GetExpression(x => x.Html_01_GeneratedViewName()) };
            yield return new object[] { "TestMailer/fixedView", this.GetExpression(x => x.Html_01_FixedViewName()) };
            yield return new object[] { null, this.GetExpression(x => x.Text_01()) };

        }

        private Expression<Func<ITestMailer, IMailerContextResult>> GetExpression(Expression<Func<ITestMailer, IMailerContextResult>> expression) => expression;




    }
}

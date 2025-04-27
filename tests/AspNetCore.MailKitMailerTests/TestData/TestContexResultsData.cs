using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailerTests.TestClients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspNetCore.MailKitMailerTests.TestData
{
    public static class TestContexResultsData
    {
        public static IEnumerable<object[]> GetTestCases()
        {
            yield return new object[] { "TestMailer/Html_01_GeneratedViewName",  GetExpression(x => x.Html_01_GeneratedViewName()) };
            yield return new object[] { "TestMailer/fixedView", GetExpression(x => x.Html_01_FixedViewName()) };
            yield return new object[] { null, GetExpression(x => x.Text_01()) };

        }
        private static Expression<Func<ITestMailer, IMailerContextResult>> GetExpression(Expression<Func<ITestMailer, IMailerContextResult>> expression) => expression;




    }
}

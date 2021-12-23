using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailerIntegrationTests.TestData
{
    public class SingleValueTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            
            yield return new object[] { new EmailAddressModel("to_test", "to_test@localhost"), null, null };
            yield return new object[] { new EmailAddressModel("to_test", "to_test@localhost"), new EmailAddressModel("cc_test", "cc_test@localhost"), null };
            yield return new object[] { new EmailAddressModel("to_test", "to_test@localhost"), new EmailAddressModel("cc_test", "cc_test@localhost"), new EmailAddressModel("bcc_test", "bcc_test@localhost") };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
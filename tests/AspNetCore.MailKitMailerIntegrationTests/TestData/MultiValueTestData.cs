using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailerIntegrationTests.TestData
{
    public class MultiValueTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            
            yield return new object[] { this.CreateAdressList("to"), null, null };
            yield return new object[] { this.CreateAdressList("to"), this.CreateAdressList("cc"), null };
            yield return new object[] { this.CreateAdressList("to"), this.CreateAdressList("cc"), this.CreateAdressList("bcc") };
        }

        private IEnumerable<EmailAddressModel> CreateAdressList(string prefix)
        {
            int amount = 10;

            for (int i =0; i<amount; i++)
            {
                yield return new EmailAddressModel($"{i}_{prefix}_test", $"{i}_{prefix}_test@localhost");
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Models
{
    public class MultiValuePayload
    {
        public IList<EmailAddressModel> To { get; set; } = new List<EmailAddressModel>();

        public IList<EmailAddressModel> Cc { get; set; } = new List<EmailAddressModel>();

        public IList<EmailAddressModel> Bcc { get; set; } = new List<EmailAddressModel>();
    }
}

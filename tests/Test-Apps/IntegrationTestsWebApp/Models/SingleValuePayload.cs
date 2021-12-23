using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTestsWebApp.Models
{
    public class SingleValuePayload
    {
        public EmailAddressModel To { get; set; }

        public EmailAddressModel Cc { get; set; }

        public EmailAddressModel Bcc { get; set; } 

        public string AttachmentFile { get; set; }

    }
}

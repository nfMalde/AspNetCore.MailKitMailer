using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Models
{
    public class AttachmentModel
    {
        public string? FilePath { get; set; }

        public Uri? FileUrl { get; set; }

        public string? ContenType { get; set; }
    }
}

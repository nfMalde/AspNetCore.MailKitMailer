using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Models
{
    public class AttachmentModel
    {
        public byte[]? FileBytes { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        public Uri? FileUrl { get; set; }

        public string? ContenType { get; set; }
    }
}

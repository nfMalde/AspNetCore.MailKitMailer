using AspNetCore.MailKitMailer.Domain;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MailKit.Net.Smtp.SmtpClient" />
    /// <seealso cref="AspNetCore.MailKitMailer.Domain.IMailkitSMTPClient" />
    public class MailkitSMTPClient:SmtpClient, IMailkitSMTPClient
    {
    }
}

using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Domain
{
    /// <summary>
    /// This interface is used to seperate the smtp client from other registered instances
    /// </summary>
    /// <seealso cref="MailKit.Net.Smtp.ISmtpClient" />
    public interface IMailkitSMTPClient: ISmtpClient
    {
    }
}

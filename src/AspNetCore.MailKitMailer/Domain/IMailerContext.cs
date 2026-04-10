using AspNetCore.MailKitMailer.Models;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Domain
{
    /// <summary>
    /// Mailer Contex
    /// </summary>
    public interface IMailerContext
    {
        /// <summary>
        /// Gets the default receipients.
        /// </summary>
        /// <value>
        /// The default receipients.
        /// </value>
        IList<EmailAddressModel> DefaultReceipients { get; }

        /// <summary>
        /// Gets the default cc receipients.
        /// </summary>
        /// <value>
        /// The default cc receipients.
        /// </value>
        IList<EmailAddressModel> DefaultCCReceipients { get; }

        /// <summary>
        /// Gets the default BCC receipients.
        /// </summary>
        /// <value>
        /// The default BCC receipients.
        /// </value>
        IList<EmailAddressModel> DefaultBCCReceipients { get; }

        /// <summary>
        /// Gets or sets from address for whole mailing contex.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        EmailAddressModel? From { get; set; }

        /// <summary>
        /// Gets or sets the SMTP configuration override.
        /// When set, this configuration will be used instead of the default SMTP configuration.
        /// </summary>
        /// <value>
        /// The SMTP configuration override, or null to use the default.
        /// </value>
        SMTPConfigModel? SmtpConfigOverride { get; set; }

        /// <summary>
        /// Called when [before send].
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        void OnBeforeSend(IServiceProvider serviceProvider);

        /// <summary>
        /// Called when [after send].
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        void OnAfterSend(IServiceProvider serviceProvider);

        /// <summary>
        /// Called to configure the SMTP client before connecting.
        /// Override this to modify SMTP client settings per mailer context (e.g., remove authentication mechanisms, set timeouts).
        /// </summary>
        /// <param name="client">The SMTP client to configure.</param>
        void OnConfigureSmtpClient(SmtpClient client);
    }
}

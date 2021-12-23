using AspNetCore.MailKitMailer.Models;
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
        EmailAddressModel From { get; set; }

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
    }
}

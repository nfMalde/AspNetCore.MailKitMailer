using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Domain
{
    /// <summary>
    /// Result returned by mail contexes
    /// </summary>
    public interface IMailerContextResult
    {
        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        string? View { get; set; }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        object? Model { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        string? Subject { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        Models.EmailAddressModel? From { get; set; }

        /// <summary>
        /// Gets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        IList<Models.EmailAddressModel> To { get;  }

        /// <summary>
        /// Gets the cc.
        /// </summary>
        /// <value>
        /// The cc.
        /// </value>
        IList<Models.EmailAddressModel> CC { get; }

        /// <summary>
        /// Gets the BCC.
        /// </summary>
        /// <value>
        /// The BCC.
        /// </value>
        IList<Models.EmailAddressModel> BCC { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this mail is HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is HTML; otherwise, <c>false</c>.
        /// </value>
        bool IsHtml { get; set; }

        /// <summary>
        /// Gets or sets the text body.
        /// </summary>
        /// <value>
        /// The text body.
        /// </value>
        string? TextBody { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>
        /// The attachments.
        /// </value>
        IAttachmentCollection? Attachments { get; set; }
    }
}

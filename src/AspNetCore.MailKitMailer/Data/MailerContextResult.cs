using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AspNetCore.MailKitMailer.Data
{
    /// <summary>
    /// Result returned by mail contexes
    /// </summary>
    /// <seealso cref="AspNetCore.MailKitMailer.Domain.IMailerContextResult" />
    public class MailerContextResult : IMailerContextResult
    {
        private IAttachmentCollection attachments = new AttachmentCollection();

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public string View { get; set; }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public object Model { get; set; }
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public EmailAddressModel From { get; set; }
        /// <summary>
        /// Gets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public IList<EmailAddressModel> To { get; } = new List<EmailAddressModel>();
        /// <summary>
        /// Gets the cc.
        /// </summary>
        /// <value>
        /// The cc.
        /// </value>
        public IList<EmailAddressModel> CC { get; } = new List<EmailAddressModel>();
        /// <summary>
        /// Gets the BCC.
        /// </summary>
        /// <value>
        /// The BCC.
        /// </value>
        public IList<EmailAddressModel> BCC { get; } = new List<EmailAddressModel>();

        /// <summary>
        /// Gets or sets a value indicating whether this mail is HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is HTML; otherwise, <c>false</c>.
        /// </value>
        public bool IsHtml { get; set; }
        /// <summary>
        /// Gets or sets the text body.
        /// </summary>
        /// <value>
        /// The text body.
        /// </value>
        public string TextBody { get; set; }

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <value>
        /// The attachments.
        /// </value>
        public IAttachmentCollection Attachments { get; set; }

         
    }
}

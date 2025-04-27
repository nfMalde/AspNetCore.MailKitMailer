using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AspNetCore.MailKitMailer.Data
{
    /// <summary>
    /// Base Abstract class for mailer contexes
    /// </summary>
    /// <seealso cref="AspNetCore.MailKitMailer.Domain.IMailerContext" />
    public abstract class MailerContextAbstract : IMailerContext
    {
        /// <summary>
        /// Gets the default receipients.
        /// </summary>
        /// <value>
        /// The default receipients.
        /// </value>
        public IList<EmailAddressModel> DefaultReceipients => new List<EmailAddressModel>();

        /// <summary>
        /// Gets the default cc receipients.
        /// </summary>
        /// <value>
        /// The default cc receipients.
        /// </value>
        public IList<EmailAddressModel> DefaultCCReceipients => new List<EmailAddressModel>();

        /// <summary>
        /// Gets the default BCC receipients.
        /// </summary>
        /// <value>
        /// The default BCC receipients.
        /// </value>
        public IList<EmailAddressModel> DefaultBCCReceipients => new List<EmailAddressModel>();

        /// <summary>
        /// Gets or sets default from address for whole mailing contex.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public EmailAddressModel? From { get; set; }

        /// <summary>
        /// Called when [after send].
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public virtual void OnAfterSend(IServiceProvider serviceProvider)
        {
             
        }

        /// <summary>
        /// Called when [before send].
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public virtual void OnBeforeSend(IServiceProvider serviceProvider)
        {
            
        }

        #region HtmlMailHelper
        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            EmailAddressModel to, 
            string subject, 
            object? model = null,
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>() { to },
                cc: null,
                bcc: null,
                from: null,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments:withAttachments
                );
        }

        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            EmailAddressModel to, 
            EmailAddressModel cc, 
            string subject,
            object? model = null, 
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null
            )
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>() { to },
                cc: new List<EmailAddressModel>() { cc },
                bcc: null,
                from: null,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments
                );
        }

        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            EmailAddressModel to, 
            EmailAddressModel cc, 
            EmailAddressModel bcc, 
            string subject, 
            object? model = null, 
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>() { to },
                cc: new List<EmailAddressModel>() { cc },
                bcc: new List<EmailAddressModel>() { bcc },
                from: null,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments
                );
        }


        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            EmailAddressModel to, 
            EmailAddressModel cc, 
            EmailAddressModel bcc, 
            EmailAddressModel from, 
            string subject, 
            object? model = null, 
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>() { to },
                cc: new List<EmailAddressModel>() { cc },
                bcc: new List<EmailAddressModel>() { bcc },
                from: from,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments
                );
        }


        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            IEnumerable<EmailAddressModel> to, 
            string subject, 
            object? model = null, 
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: null,
                bcc: null,
                from: null,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments
                );
        }

        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            IEnumerable<EmailAddressModel> to, 
            IEnumerable<EmailAddressModel> cc, 
            string subject, 
            object? model = null, 
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: new List<EmailAddressModel>(cc),
                bcc: null,
                from: null,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments

                );
        }

        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            IEnumerable<EmailAddressModel> to, 
            IEnumerable<EmailAddressModel> cc, 
            IEnumerable<EmailAddressModel> bcc, 
            string subject, 
            object? model = null, 
            string? viewName = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: new List<EmailAddressModel>(cc),
                bcc: new List<EmailAddressModel>(bcc),
                from: null,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments
                );
        }

        /// <summary>
        /// Return an HTML Contex Result for mails
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="withAttachments">Adds attachments to the mail context.</param>
        /// <returns></returns>
        protected IMailerContextResult HtmlMail(
            IEnumerable<EmailAddressModel> to, 
            IEnumerable<EmailAddressModel> cc, 
            IEnumerable<EmailAddressModel> bcc, 
            EmailAddressModel from, 
            string subject, 
            object? model = null, 
            string? viewName = null, 
            Action<IAttachmentCollection>? withAttachments = null
            )
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: new List<EmailAddressModel>(cc),
                bcc: new List<EmailAddressModel>(bcc),
                from: from,
                subject: subject,
                plainBody: null,
                view: viewName,
                isHtml: true,
                model: model,
                withAttachments: withAttachments
                );
        }


        #endregion HtmlMailHelper

        #region TextMailHelper
        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">Adds attachments to the mail context .</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            EmailAddressModel to, 
            string subject,
            string text, 
            object? model = null, 
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
               to: new List<EmailAddressModel>() { to },
               cc: null,
               bcc: null,
               from: null,
               subject: subject,
               plainBody: text,
               view: null,
               isHtml: false,
               model: model,
               withAttachments : withAttachments

               );
        }

        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">Adds attach attachments to the mail contex.</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            EmailAddressModel to,
            EmailAddressModel cc,
            string subject,
            string text,
            object? model = null,
            Action<IAttachmentCollection>? withAttachments = null
            )
        {
            return this._CreateResult(
               to: new List<EmailAddressModel>() { to },
               cc: new List<EmailAddressModel>() { cc },
               bcc: null,
               from: null,
               subject: subject,
               plainBody: text,
               view: null,
               isHtml: false,
               model: model,
               withAttachments: withAttachments
               );
        }

        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">The with attachments.</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            EmailAddressModel to, 
            EmailAddressModel cc, 
            EmailAddressModel bcc, 
            string subject,
            string text,
            object? model = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
               to: new List<EmailAddressModel>() { to },
               cc: new List<EmailAddressModel>() { cc },
               bcc: new List<EmailAddressModel>() { bcc },
               from: null,
               subject: subject,
               plainBody: text,
               view: null,
               isHtml: false,
               model: model,
               withAttachments: withAttachments
               );
        }
        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">The with attachments.</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            EmailAddressModel to, 
            EmailAddressModel cc, 
            EmailAddressModel bcc, 
            EmailAddressModel from, 
            string subject, 
            string text, 
            object? model = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
               to: new List<EmailAddressModel>() { to },
               cc: new List<EmailAddressModel>() { cc },
               bcc: new List<EmailAddressModel>() { bcc },
               from: from,
               subject: subject,
               plainBody: text,
               view: null,
               isHtml: false,
               model: model,
               withAttachments: withAttachments
               );
        }

        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">Adds attachments to the mail.</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            IEnumerable<EmailAddressModel> to, 
            string subject, 
            string text, 
            object? model = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: null,
                bcc: null,
                from: null,
                subject: subject,
                plainBody: text,
                view: null,
                isHtml: false,
                model: model,
                withAttachments: withAttachments
                );
        }

        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">Adds attachments to the mail.</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            IEnumerable<EmailAddressModel> to, 
            IEnumerable<EmailAddressModel> cc, 
            IEnumerable<EmailAddressModel> bcc, 
            string subject, 
            string text, 
            object? model = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: new List<EmailAddressModel>(cc),
                bcc: new List<EmailAddressModel>(bcc),
                from: null,
                subject: subject,
                plainBody: text,
                view: null,
                isHtml: false,
                model: model,
                withAttachments: withAttachments
               
                );
        }

        /// <summary>
        /// Returns an Plain Text Mail Result for the contex
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="withAttachments">Adds attachments to the mail</param>
        /// <returns></returns>
        protected IMailerContextResult PlainTextMail(
            IEnumerable<EmailAddressModel> to, 
            IEnumerable<EmailAddressModel> cc, 
            IEnumerable<EmailAddressModel> bcc, 
            EmailAddressModel from, 
            string subject, 
            string text, 
            object? model = null,
            Action<IAttachmentCollection>? withAttachments = null)
        {
            return this._CreateResult(
                to: new List<EmailAddressModel>(to),
                cc: new List<EmailAddressModel>(cc),
                bcc: new List<EmailAddressModel>(bcc),
                from: from,
                subject: subject,
                plainBody: text,
                view: null,
                isHtml: false,
                model: model,
                withAttachments: withAttachments
                );
        }

        #endregion TextMailHelper

        #region ResultFactory
        /// <summary>
        /// Creates the result.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="plainBody">The plain body.</param>
        /// <param name="view">The view.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private IMailerContextResult _CreateResult(
            IEnumerable<EmailAddressModel> to, 
            IEnumerable<EmailAddressModel>? cc,
            IEnumerable<EmailAddressModel>? bcc,
            EmailAddressModel? from,
            string subject,
            string? plainBody,
            string? view,
            bool isHtml,
            object? model,
            Action<IAttachmentCollection>? withAttachments = null
            )
        {
            MailerContextResult r = new MailerContextResult();
            /// Make header
            /// 
            if (to != null)
            {
                to.Where(x => x != null).ToList().ForEach(x => r.To.Add(x));
            }

           // to?.ToList().ForEach(x => r.To.Add(x));
            
            cc?.Where(x => x != null).ToList().ForEach(x => r.CC.Add(x));
            bcc?.Where(x => x != null).ToList().ForEach(x => r.BCC.Add(x));
            r.From = from;
            r.IsHtml = isHtml;
            r.Subject = subject;
            r.View = view;
            r.TextBody = plainBody;
            r.Model = model;

            if (withAttachments != null)
            {
                 
                AttachmentCollection attachments = new AttachmentCollection(); 
                withAttachments(attachments);
                r.Attachments = attachments;
            }

            return r;
        }

        #endregion ResultFactory
    }
}

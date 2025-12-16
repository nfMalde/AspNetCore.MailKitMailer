using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Helper;
using AspNetCore.MailKitMailer.Models;
using HtmlAgilityPack;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.MailKitMailer.Data
{
    /// <summary>
    /// Mail Client used for sending the prepared mail messages from MailerContexes
    /// </summary>
    /// <seealso cref="AspNetCore.MailKitMailer.Domain.IMailClient" />
    public class MailClient : IMailClient
    {
        /// <summary>
        /// The SMTP configuration
        /// </summary>
        private readonly IOptions<SMTPConfigModel> smtpConfig;
        /// <summary>
        /// The client
        /// </summary>
        private readonly IMailkitSMTPClient client;

        /// <summary>
        /// The service provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;
        /// <summary>
        /// The razor view engine
        /// </summary>
        private readonly IMailerViewEngine razorViewEngine;
        /// <summary>
        /// The temporary data provider
        /// </summary>
        private readonly ITempDataProvider tempDataProvider;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor? httpContextAccessor;

        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailClient" /> class.
        /// </summary>
        /// <param name="smtpConfig">The SMTP configuration.</param>
        /// <param name="client">The client.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="razorViewEngine">The razor view engine.</param>
        /// <param name="tempDataProvider">The temporary data provider.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MailClient(
            IOptions<Models.SMTPConfigModel> smtpConfig,
            IMailkitSMTPClient client,
            IServiceProvider serviceProvider,
            IMailerViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IEnumerable<IHttpContextAccessor> httpContextAccessor,
            IHttpClientFactory httpClientFactory)
        {
            this.smtpConfig = smtpConfig;
            this.client = client;
            this.serviceProvider = serviceProvider;
            this.razorViewEngine = razorViewEngine;
            this.tempDataProvider = tempDataProvider;
            this.httpContextAccessor = httpContextAccessor?.Count() > 0 ? httpContextAccessor.FirstOrDefault() : null;
            this.httpClient = httpClientFactory.CreateClient("attachmentDownloader");
        }


        /// <summary>
        /// Sends the message prepared by given mailer <typeparam name="TContext"> Mailer Contex
        /// </summary>
        /// <typeparam name="TContext">The type of the mailer context.</typeparam>
        /// <param name="contextBuilder">The context builder.</param>
        public void Send<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext
        {
            this.SendAsync(contextBuilder).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends the message prepared by given mailer <typeparam name="TContext"> Mailer Contex asynchronous.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="contextBuilder">The context builder.</param>
        /// <returns></returns>
        public async Task SendAsync<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext
        {
            TContext? ctx = this.serviceProvider.GetService(typeof(TContext)) as TContext;

            if (ctx == null)
            {
                throw new Exception($"Mailer Contex of type {typeof(TContext)} were not found.");
            }


            ctx.OnBeforeSend(this.serviceProvider);

            IMailerContextResult result = this._CompileMailerContext<TContext>(contextBuilder);
            MimeMessage message = await this.PrepareMessage(this.serviceProvider.GetRequiredService<TContext>(), result);

            this.client.CheckCertificateRevocation = this.smtpConfig.Value?.CheckCertificateRevocation ?? true;

            await this.client.ConnectAsync(this.smtpConfig.Value?.Host, this.smtpConfig?.Value?.Port ?? 25, this.smtpConfig?.Value?.UseSSL ?? false);

            if (this.smtpConfig?.Value?.DoAuthenticate ?? false)
            {
                await this.client.AuthenticateAsync(this.smtpConfig.Value.Username, this.smtpConfig.Value.Password);
            }

            await this.client.SendAsync(message);
            await this.client.DisconnectAsync(true);

            ctx.OnAfterSend(this.serviceProvider);

        }
        /// <summary>
        /// Asynchronously gets the content of the email based on the provided context.
        /// </summary>
        /// <typeparam name="TContext">The type of the mailer context.</typeparam>
        /// <param name="contextBuilder">The context builder expression.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the email content as a string.</returns>
        public async Task<string?> GetContentAsync<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext
        {
            TContext? ctx = this.serviceProvider.GetService(typeof(TContext)) as TContext;
           
            if (ctx == null)
            {
                throw new Exception($"Mailer Contex of type {typeof(TContext)} were not found.");
            }

            IMailerContextResult result = this._CompileMailerContext<TContext>(contextBuilder);

           return  await this._RenderView(result, ctx);    

        }

        /// <summary>
        /// Prepares the <see cref="MimeMessage"/> message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private async Task<MimeMessage> PrepareMessage(IMailerContext context, IMailerContextResult result)
        {
            MimeMessage message = new MimeKit.MimeMessage();
            message.Subject = result.Subject;

            EmailAddressModel from = result.From ?? context.From ?? this.smtpConfig.Value?.FromAddress ?? new EmailAddressModel() { Email = "root@localhost", Name = "Root" };
            
            List< EmailAddressModel> to =  new List<EmailAddressModel>(result.To);
            List<EmailAddressModel> cc = new List<EmailAddressModel>(result.CC);
            List<EmailAddressModel> bcc = new List<EmailAddressModel>(result.BCC);

            if (context.DefaultReceipients != null)
            {
                to.AddRange(context.DefaultReceipients);
            }
             
            if (context.DefaultCCReceipients != null)
            {
                cc.AddRange(context.DefaultCCReceipients);
            }


            if (context.DefaultBCCReceipients != null)
            {
                bcc.AddRange(context.DefaultBCCReceipients);
            }

            //From
            message.From.Add(new MailboxAddress(from.Name, from.Email));

            // To
            message.To.AddRange(to.Select(x => new MailboxAddress(x.Name, x.Email)));

            // CC
            message.Cc.AddRange(cc.Select(x => new MailboxAddress(x.Name, x.Email)));

            // Add defaults for cc
            message.Bcc.AddRange(bcc.Select(x => new MailboxAddress(x.Name, x.Email)));
             
            context.DefaultBCCReceipients?.ToList().ForEach(addr => message.Bcc.Add(new MailboxAddress(addr.Name, addr.Email)));
             
            // Render the view
            string htmlBody = await this._RenderView(result, context);
            string textBody = result.TextBody ?? "";

            if (string.IsNullOrEmpty(textBody))
            {
                textBody = _HtmlToPlainText(htmlBody);
            }

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlBody;
            bodyBuilder.TextBody = textBody;
           
    
            // Attachments
            if (result.Attachments != null && !result.Attachments.IsEmpty())
            {
                foreach(var attachment in result.Attachments)
                {
                    if (attachment.FilePath != null)
                    {
                        MimePart? att = null;

                        if (attachment.ContenType != null)
                        {
                            att = new MimePart(attachment.ContenType);
                        } else
                        {
                            att = new MimePart(MimeKit.MimeTypes.GetMimeType(attachment.FilePath));
                        }

                        att.Content = new MimeContent(File.OpenRead(attachment.FilePath), ContentEncoding.Default);
                        att.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                        att.ContentTransferEncoding = ContentEncoding.Base64;
                        att.FileName = Path.GetFileName(attachment.FilePath);

                        bodyBuilder.Attachments.Add(att);
                    }
                    else if (attachment.FileUrl != null)
                    {
                        
                        var data = await this.httpClient.GetByteArrayAsync(attachment.FileUrl);
                        var content = new System.IO.MemoryStream(data);
                        string fname = Path.GetFileName(attachment.FileUrl.ToString());

                        if (!Path.HasExtension(fname))
                        {
                            // We got no valid file name. Lets see if we find an content disposition.
                            var rr = await this.httpClient.GetAsync(attachment.FileUrl);
                            var headers = rr.Content.Headers;

                            if (headers != null && headers.ContentDisposition != null)
                            {
                                string? cdname = headers.ContentDisposition.FileName;

                                if (!string.IsNullOrEmpty(cdname))
                                {
                                    fname = cdname;
                                }
                            }

                        }

                        MimePart? att = null;

                        if (attachment.ContenType != null)
                        {
                            att = new MimePart(attachment.ContenType);
                        }
                        else
                        {
                            att = new MimePart(MimeKit.MimeTypes.GetMimeType(fname));
                        }

                        att.Content = new MimeContent(content, ContentEncoding.Default);
                        att.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                        att.ContentTransferEncoding = ContentEncoding.Base64;
                        att.FileName = fname;

                        bodyBuilder.Attachments.Add(att);
                    }
                }
            }


            message.Body = bodyBuilder.ToMessageBody();


            return message;
        }

        /// <summary>
        /// Converts  HTML content to plain text for the additional text body.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        private string _HtmlToPlainText(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            StringWriter writer = new StringWriter();

            this._ExtractText(doc.DocumentNode, writer);

            return writer.ToString();
        }

        /// <summary>
        /// Extracts the text of the given html node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="writer">The writer.</param>
        private void _ExtractText(HtmlNode node, StringWriter writer)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    return;

                case HtmlNodeType.Text:

                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    string html = ((HtmlTextNode)node).Text;

                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        writer.Write(HtmlEntity.DeEntitize(html));
                    }

                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            writer.Write("\r\n\n");
                            break;
                        case "br":
                            writer.Write("\r\n");
                            break;
                    }

                    break;
            }


            if (node.HasChildNodes)
            {
                node.ChildNodes.ToList()
                    .ForEach(n => this._ExtractText(n, writer));
            }
        }

        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="mailerContext">The mailer context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        private async Task<string> _RenderView(IMailerContextResult result, IMailerContext mailerContext)
        {
            var httpContext =  this.httpContextAccessor?.HttpContext ?? new DefaultHttpContext() { RequestServices = this.serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
           
            // When text, we dont need any view rendering if we got a text body
            if (!result.IsHtml && !string.IsNullOrEmpty(result.TextBody) && result.View == null)
            {
                return result.TextBody;
            }

            using (var sw = new StringWriter())
            {
                string contextName = mailerContext.GetType().Name;
                
                var viewResult = this.razorViewEngine.FindView(actionContext, $"{contextName}/{result.View}", true);
                
                if (viewResult?.View == null)
                {
                    viewResult = this.razorViewEngine.FindView(actionContext, $"{result.View}", true);
                }

              
                if (viewResult == null)
                {
                    throw new Exception($"View not found by names {result.View}");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = result.Model

                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View!,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, this.tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );
    
                await viewResult.View!.RenderAsync(viewContext);

                return sw.ToString();
            }

        }

        /// <summary>
        /// Compiles the mailer context expression.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Invalid MailerContext Expression. Expecting call to Method and not static values. E.g. ::Send<IMyMailerContext>(x => x.HelloMail()).
        /// or
        /// Unable to load MailerContext {typeof(TContext).FullName}. Service Provider returned null.
        /// </exception>
        private IMailerContextResult _CompileMailerContext<TContext>(Expression<Func<TContext, IMailerContextResult>> expression) where TContext : class, IMailerContext
        {
            MethodCallExpression? exp = expression.Body as MethodCallExpression;

            if (exp == null)
            {
                throw new Exception("Invalid MailerContext Expression. Expecting call to Method and not static values. E.g. ::Send<IMyMailerContext>(x => x.HelloMail()).");
            } 

            string defaultViewName = exp.Method.Name;
           

            // Load Mailer Context
            TContext? mailerContext = this.serviceProvider.GetService(typeof(TContext)) as TContext;

            if (mailerContext == null)
            {
                throw new Exception($"Unable to load MailerContext {typeof(TContext).FullName}. Service Provider returned null.");
            }

            // Compile expression

            IMailerContextResult r = expression.Compile()(mailerContext);

            if (string.IsNullOrEmpty(r.View) && (string.IsNullOrWhiteSpace(r.TextBody) || r.IsHtml))
            {
                r.View = defaultViewName; 
            } 


            return r;
        }
    }
}

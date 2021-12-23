using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace AspNetCore.MailKitMailer.Models
{
    /// <summary>
    /// Options for the Mailer View Engine
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions" />
    public class MailerViewEngineOptions: RazorViewEngineOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailerViewEngineOptions"/> class.
        /// </summary>
        public MailerViewEngineOptions()
        {
            /// Setup lookup locations
            this.ViewLocationFormats.Clear();
            this.ViewLocationFormats.Add("~/Mailer-Views/{0}" + RazorViewEngine.ViewExtension);

            this.ViewLocationFormats.Add("~/Mailer-Views/{1}/{0}" + RazorViewEngine.ViewExtension);
            this.ViewLocationFormats.Add("~/Views/Mailer/{1}/{0}" + RazorViewEngine.ViewExtension);
            this.ViewLocationFormats.Add("~/Mailer-Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            
            this.AreaViewLocationFormats.Add("/Areas/{2}/Mailer-Views/{1}/{0}" + RazorViewEngine.ViewExtension);
            this.AreaViewLocationFormats.Add("/Areas/{2}/Mailer-Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            this.AreaViewLocationFormats.Add("/Areas/{2}/Mailer/{1}/{0}" + RazorViewEngine.ViewExtension);
            this.AreaViewLocationFormats.Add("/Areas/{2}/Mailer/Shared/{0}" + RazorViewEngine.ViewExtension);

            this.AreaViewLocationFormats.Add("/Mailer-Views/Shared/{0}" + RazorViewEngine.ViewExtension);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Domain
{
    /// <summary>
    /// Mailer View Engine
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Razor.IRazorViewEngine" />
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ViewEngines.IViewEngine" />
    public interface IMailerViewEngine : IRazorViewEngine, IViewEngine
    {
    }
}

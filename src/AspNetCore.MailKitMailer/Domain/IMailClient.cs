using AspNetCore.MailKitMailer.Domain;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.MailKitMailer.Domain
{
    public interface IMailClient
    {
        /// <summary>
        /// Sends the message prepared by given mailer <typeparam name="TContext"> Mailer Contex
        /// </summary>
        /// <typeparam name="TContext">The type of the mailer context.</typeparam>
        /// <param name="contextBuilder">The context builder.</param>
        void Send<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext;
        /// <summary>
        /// Sends the message prepared by given mailer <typeparam name="TContext"> Mailer Contex asynchronous.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="contextBuilder">The context builder.</param>
        /// <returns></returns>
        Task SendAsync<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext;
    }
}
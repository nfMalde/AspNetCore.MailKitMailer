using AspNetCore.MailKitMailer.Domain;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.MailKitMailer.Domain
{
    /// <summary>
    /// Interface for Mail Client to handle email sending and content retrieval.
    /// </summary>
    public interface IMailClient
    {
        /// <summary>
        /// Asynchronously gets the content of the email based on the provided context.
        /// </summary>
        /// <typeparam name="TContext">The type of the mailer context.</typeparam>
        /// <param name="contextBuilder">The context builder expression.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the email content as a string.</returns>
        Task<string?> GetContentAsync<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext;

        /// <summary>
        /// Sends the message prepared by the given mailer context.
        /// </summary>
        /// <typeparam name="TContext">The type of the mailer context.</typeparam>
        /// <param name="contextBuilder">The context builder expression.</param>
        void Send<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext;

        /// <summary>
        /// Asynchronously sends the message prepared by the given mailer context.
        /// </summary>
        /// <typeparam name="TContext">The type of the mailer context.</typeparam>
        /// <param name="contextBuilder">The context builder expression.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendAsync<TContext>(Expression<Func<TContext, IMailerContextResult>> contextBuilder) where TContext : class, IMailerContext;
    }
}
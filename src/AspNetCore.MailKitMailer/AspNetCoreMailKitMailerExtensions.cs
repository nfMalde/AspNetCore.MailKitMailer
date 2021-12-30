using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using AspNetCore.MailKitMailer.Models;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MailKit.Net.Smtp;
using System.Net.Http;

namespace AspNetCore.MailKitMailer
{
    /// <summary>
    /// AspNetCoreMailKitMailerExtensions
    /// </summary>
    public static class AspNetCoreMailKitMailerExtensions
    {
        /// <summary>
        /// Adds the ASP net core mail kit mailer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="configureClient">Action to configure the smtp client defaults.</param>
        /// <returns></returns>
        public static IServiceCollection AddAspNetCoreMailKitMailer(this IServiceCollection services, IConfiguration configuration, Action<SmtpClient> configureClient = null)
        {
            services = CheckForHttpClient(services);
            services.Configure<Models.SMTPConfigModel>(x => configuration.GetSection("MailKitMailer").Bind(x));
            services.Configure<Models.MailerViewEngineOptions>(x => x = new Models.MailerViewEngineOptions());
            services.AddScoped<IMailerViewEngine, MailerViewEngine>();
            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<IMailkitSMTPClient>(x => {
                MailkitSMTPClient client = new MailkitSMTPClient();

                if (configureClient != null)
                {
                    configureClient(client);
                }

                return client as IMailkitSMTPClient;
            });
            return services;
        }

        /// <summary>
        /// Adds the ASP net core mail kit mailer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="smtpconfig">The smtpconfig.</param>
        /// <param name="configureClient">The configure client.</param>
        /// <returns></returns>
        public static IServiceCollection AddAspNetCoreMailKitMailer(this IServiceCollection services, Models.SMTPConfigModel smtpconfig, Action<SmtpClient> configureClient = null)
        {
            services = CheckForHttpClient(services);
            services.Configure<Models.SMTPConfigModel>(x => {
                x.GetType().GetProperties()
                .ToList().ForEach(p =>
                {
                    if (smtpconfig.GetType().GetProperty(p.Name) != null)
                    {
                        p.SetValue(x, smtpconfig.GetType().GetProperty(p.Name).GetValue(smtpconfig));
                    }
                }); 
            });
            services.Configure<Models.MailerViewEngineOptions>(x => x = new Models.MailerViewEngineOptions());
            services.AddScoped<IMailerViewEngine, MailerViewEngine>();
            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<IMailkitSMTPClient>(x => {
                MailkitSMTPClient client = new MailkitSMTPClient();

                if (configureClient != null)
                {
                    configureClient(client);
                }

                return client as IMailkitSMTPClient;
            });

            return services;
        }


        /// <summary>
        /// Adds the ASP net core mail kit mailer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="fromAddress">From address.</param>
        /// <param name="fromName">From name.</param>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        /// <param name="UseSSL">if set to <c>true</c> [use SSL].</param>
        /// <returns></returns>
        public static IServiceCollection AddAspNetCoreMailKitMailer(this IServiceCollection services, string fromAddress, string fromName, string host, string username, string password, int port, bool UseSSL, Action<SmtpClient> configureClient = null)
        {
            services = CheckForHttpClient(services);
            services.Configure<Models.SMTPConfigModel>(x => x = new Models.SMTPConfigModel()
            {
                FromAddress = new Models.EmailAddressModel()
                {
                    Name = fromName,
                    Email = fromAddress,
                    
                },
                Host = host,
                Port = port,
                UseSSL = UseSSL,
                Username = username,
                Password = password
            });
            services.Configure<Models.MailerViewEngineOptions>(x => x = new Models.MailerViewEngineOptions());
            services.AddScoped<IMailerViewEngine, MailerViewEngine>();
            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<IMailkitSMTPClient>(x => {
                MailkitSMTPClient client = new MailkitSMTPClient();

                if (configureClient != null)
                {
                    configureClient(client);
                }

                return client as IMailkitSMTPClient;
            });

            return services;
        }

        /// <summary>
        /// Registers all mail contexes.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAllMailContexesOfCallingAssembly(this IServiceCollection  services)
        {
            return RegisterAllMailContexesOfAssembly(services, Assembly.GetCallingAssembly());
        }


        /// <summary>
        /// Registers the type of all mail contex of assembly containing given type.
        /// </summary>
        /// <typeparam name="TContaining">The type of the containing.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAllMailContexOfAssemblyContainingType<TContaining>(this IServiceCollection services)
        {
            return RegisterAllMailContexesOfAssembly(services, Assembly.GetAssembly(typeof(TContaining)));
        }



        /// <summary>
        /// Registers all mail contexes of assembly.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAllMailContexesOfAssembly(this IServiceCollection services, Assembly assembly)
        {
            var q = assembly
               .GetTypes().Where(x => !x.IsAbstract
               && x.IsSubclassOf(typeof(MailerContextAbstract))

               ).ToList();

            foreach (var contextCls in q)
            {
                var interfaces = contextCls.GetInterfaces();

                if (interfaces.Any(x => x != typeof(IMailerContext) && typeof(IMailerContext).IsAssignableFrom(x)))
                {
                    var desiredInterface = interfaces.First(x => x != typeof(IMailerContext) && typeof(IMailerContext).IsAssignableFrom(x));

                    services.AddScoped(desiredInterface, contextCls);
                }
                else
                {
                    services.AddScoped(contextCls);
                }
            }

            return services;
        }

        private static IServiceCollection CheckForHttpClient(IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(IHttpClientFactory)))
            {
                services.AddHttpClient();
            }

            return services;
        }
    }
}

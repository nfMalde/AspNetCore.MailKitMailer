﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Helper
{
    /// <summary>
    /// <see cref="IUrlHelper"/> extension methods.
    /// 
    /// </summary>
    /// <remarks>
    /// From: https://stackoverflow.com/questions/30755827/getting-absolute-urls-using-asp-net-core
    /// Renamed it to prevent conflicts.
    /// </remarks>
    public static class AspNetCoreMimeKitMailerUrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and
        /// route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string? MailerAbsoluteAction(
            this IUrlHelper url,
            string actionName,
            string controllerName,
            object? routeValues = null)
        {
            return url.Action(actionName, controllerName, routeValues, url.ActionContext.HttpContext.Request.Scheme);
        }

        /// <summary>
        /// Generates a fully qualified URL to the specified content by using the specified content path. Converts a
        /// virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The absolute URL.</returns>
        public static string MailerAbsoluteContent(
            this IUrlHelper url,
            string contentPath)
        {
            HttpRequest request = url.ActionContext.HttpContext.Request;
            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
        }

        /// <summary>
        /// Generates a fully qualified URL to the specified route by using the route name and route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string? MailerAbsoluteRouteUrl(
            this IUrlHelper url,
            string routeName,
            object? routeValues = null)
        {
            return url.RouteUrl(routeName, routeValues, url.ActionContext.HttpContext.Request.Scheme);
        }
    }
}

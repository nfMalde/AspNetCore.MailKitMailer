using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.MailKitMailer.TagHelpers
{
    /// <summary>
    /// Helper for adding css files inline to prevent mail clients from blocking them.
    /// 
    /// </summary>
    /// 
    /// <remarks>Based on https://www.meziantou.net/inlining-a-stylesheet-using-a-taghelper-in-asp-net-core.htm .
    /// Does *NOT* rewrite imports / image paths in the css files. For this kind of action I highly recommend libraries like WebOptimizer.</remarks>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class MailerInlineStyleTagHelper : TagHelper
    {
        /// <summary>
        /// The hosting environment
        /// </summary>
        private readonly IWebHostEnvironment hostingEnvironment;
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// This is the internal class for an cache entry used in ditributed caches
        /// </summary>
        private class DCacheEntry
        {
            /// <summary>
            /// Gets or sets the cache date.
            /// </summary>
            /// <value>
            /// The cache date.
            /// </value>
            public DateTime CacheDate { get; set; }

            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public string Content { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailerInlineStyleTagHelper"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public MailerInlineStyleTagHelper(IWebHostEnvironment hostingEnvironment, IServiceProvider serviceProvider)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>
        /// The href.
        /// </value>
        [HtmlAttributeName("href")]
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use content root].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use content root]; otherwise, <c>false</c>.
        /// </value>
        [HtmlAttributeName("use-content-root")]
        public bool UseContentRoot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [force memory cache].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force memory cache]; otherwise, <c>false</c>.
        /// </value>
        [HtmlAttributeName("force-memory-cache")]
        public bool ForceMemoryCache { get; set; }

        /// <summary>
        /// Asynchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <remarks>
        /// By default this calls into <see cref="M:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper.Process(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext,Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput)" />.
        /// </remarks>
        /// .
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string path = Href;
            string cachekey = "AspNetCoreMailKitMailer__InlineStyleTagHelper-" + path;

            string fileContent = null;
            // Get the value from the cache, or compute the value and add it to the cache
            IMemoryCache cache = this.serviceProvider.GetService(typeof(IMemoryCache)) as IMemoryCache;
            IDistributedCache distributedCache = this.serviceProvider.GetService(typeof(IDistributedCache)) as IDistributedCache;
            IFileProvider fileProvider = this.UseContentRoot ? this.hostingEnvironment.ContentRootFileProvider : this.hostingEnvironment.WebRootFileProvider;
            IFileInfo file = fileProvider.GetFileInfo(path);

            if ((this.ForceMemoryCache || distributedCache == null) && cache != null)
            {
                fileContent = await cache.GetOrCreateAsync(cachekey, async entry =>
                {
                    return await this._ManageCacheEntry(entry, path);
                });

            }
            else if (distributedCache != null)
            {
                string _entryContent = await distributedCache.GetStringAsync(cachekey);

                var entry = !string.IsNullOrEmpty(_entryContent) ? JsonConvert.DeserializeObject<DCacheEntry>(_entryContent) : null;

                fileContent = entry?.Content;


                if (entry == null || file.LastModified.LocalDateTime > entry.CacheDate || fileContent == null || string.IsNullOrEmpty(fileContent))
                {


                    if (file == null || !file.Exists)
                    {
                        output.SuppressOutput();
                        return;
                    }

                    fileContent = await ReadFileContent(file);


                    string obj = JsonConvert.SerializeObject(new DCacheEntry()
                    {
                        CacheDate = file.LastModified.LocalDateTime,
                        Content = fileContent
                    });

                    await distributedCache.SetStringAsync(cachekey, obj);
                }
            }
            else
            {
                // No caching enabled
                fileContent = await ReadFileContent(file);
            }


            if (fileContent == null)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "style";
            output.Attributes.RemoveAll("href");
            output.Content.AppendHtml(fileContent);
        }


        /// <summary>
        /// Reads the content of the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private static async Task<string> ReadFileContent(IFileInfo file)
        {
            using (var stream = file.CreateReadStream())
            using (var textReader = new StreamReader(stream))
            {
                return await textReader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Manages the cache entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private async Task<string> _ManageCacheEntry(ICacheEntry entry, string path)
        {

            IFileProvider fileProvider = this.hostingEnvironment.WebRootFileProvider;

            IChangeToken changeToken = fileProvider.Watch(path);

            entry.SetPriority(CacheItemPriority.NeverRemove);
            entry.AddExpirationToken(changeToken);

            IFileInfo file = fileProvider.GetFileInfo(path);
            if (file == null || !file.Exists)
                return null;

            return await ReadFileContent(file);
        }
    }
}

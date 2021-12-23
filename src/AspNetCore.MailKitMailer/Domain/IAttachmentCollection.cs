using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections.Generic;

namespace AspNetCore.MailKitMailer.Domain
{
    /// <summary>
    /// This collection is used to store attachments with the correct data
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable&lt;AspNetCore.MailKitMailer.Models.AttachmentModel&gt;" />
    public interface IAttachmentCollection
    {
        /// <summary>
        /// The attachments
        /// </summary>
        IAttachmentCollection Add(string filePath);
        /// <summary>
        /// Adds the specified attachment by file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        IAttachmentCollection Add(string filepath, string contentType);
        /// <summary>
        /// Adds the specified attachment by filepath and sets the content type.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="contentType">Type of the content.</param>
        IAttachmentCollection Add(Uri url);
        /// <summary>
        /// Adds the specified URL by url  and sets the content type.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="contentType">Type of the content.</param>
        IAttachmentCollection Add(Uri url, string contentType);
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator<AttachmentModel> GetEnumerator();

        /// <summary>
        /// Converts to list.
        /// </summary>
        /// <returns></returns>
        IList<AttachmentModel> ToList();

        bool IsEmpty();
    }
}
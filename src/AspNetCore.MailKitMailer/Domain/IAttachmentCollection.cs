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
        IAttachmentCollection Add(string filePath, string? fileName = null);

        IAttachmentCollection Add(byte[] fileBytes, string fileName, string contentType);
        /// <summary>
        /// Adds the specified attachment by file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        IAttachmentCollection Add(string filepath, string contentType, string? fileName = null);
        /// <summary>
        /// Adds the specified attachment by filepath and sets the content type.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="contentType">Type of the content.</param>
        IAttachmentCollection Add(Uri url, string? fileName = null);
        /// <summary>
        /// Adds the specified URL by url  and sets the content type.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="contentType">Type of the content.</param>
        IAttachmentCollection Add(Uri url, string contentType, string? fileName = null);

        /// <summary>
        /// Adds a linked resource (inline attachment) with a Content-ID for use in HTML emails.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="fileBytes">The file content as byte array.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        IAttachmentCollection AddLinkedResource(byte[] fileBytes, string fileName, string contentType, string contentId);

        /// <summary>
        /// Adds a linked resource (inline attachment) from a file path with a Content-ID.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        IAttachmentCollection AddLinkedResource(string filePath, string contentId, string? fileName = null);

        /// <summary>
        /// Adds a linked resource (inline attachment) from a file path with a Content-ID and content type.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        IAttachmentCollection AddLinkedResource(string filePath, string contentType, string contentId, string? fileName = null);

        /// <summary>
        /// Adds a linked resource (inline attachment) from a URL with a Content-ID.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="url">The URL to download the file from.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        IAttachmentCollection AddLinkedResource(Uri url, string contentId, string? fileName = null);

        /// <summary>
        /// Adds a linked resource (inline attachment) from a URL with a Content-ID and content type.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="url">The URL to download the file from.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        IAttachmentCollection AddLinkedResource(Uri url, string contentType, string contentId, string? fileName = null);

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
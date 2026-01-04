using AspNetCore.MailKitMailer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AspNetCore.MailKitMailer.Domain;

namespace AspNetCore.MailKitMailer.Data
{
    /// <summary>
    /// This collection is used to store attachments with the correct data
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable&lt;AspNetCore.MailKitMailer.Models.AttachmentModel&gt;" />
    public class AttachmentCollection : IEnumerable<Models.AttachmentModel>, IAttachmentCollection
    {
        /// <summary>
        /// The attachments
        /// </summary>
        private IList<AttachmentModel> attachments = new List<AttachmentModel>();

        public IAttachmentCollection Add(byte[] fileBytes, string fileName, string contentType)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FileBytes = fileBytes,
                FileName = fileName,
                ContenType = contentType
            });
            return this;
        }

        /// <summary>
        /// Adds the specified attachment by file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public IAttachmentCollection Add(string filePath, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FilePath = filePath,
                FileName = fileName
            });

            return this;
        }

        /// <summary>
        /// Adds the specified attachment by filepath and sets the content type.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="contentType">Type of the content.</param>
        public IAttachmentCollection Add(string filepath, string contentType, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                ContenType = contentType,
                FilePath = filepath,
                FileName = fileName
            });

            return this;
        }

        /// <summary>
        /// Adds the specified attachment by URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public IAttachmentCollection Add(Uri url, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FileUrl = url,
                FileName = fileName
            });

            return this;
        }

        /// <summary>
        /// Adds the specified URL by url  and sets the content type.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="contentType">Type of the content.</param>
        public IAttachmentCollection Add(Uri url, string contentType, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                ContenType = contentType,
                FileUrl = url,
                FileName = fileName
            });

            return this;
        }

        /// <summary>
        /// Adds a linked resource (inline attachment) with a Content-ID for use in HTML emails.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="fileBytes">The file content as byte array.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        public IAttachmentCollection AddLinkedResource(byte[] fileBytes, string fileName, string contentType, string contentId)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FileBytes = fileBytes,
                FileName = fileName,
                ContenType = contentType,
                ContentId = contentId
            });
            return this;
        }

        /// <summary>
        /// Adds a linked resource (inline attachment) from a file path with a Content-ID.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        public IAttachmentCollection AddLinkedResource(string filePath, string contentId, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FilePath = filePath,
                FileName = fileName,
                ContentId = contentId
            });
            return this;
        }

        /// <summary>
        /// Adds a linked resource (inline attachment) from a file path with a Content-ID and content type.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        public IAttachmentCollection AddLinkedResource(string filePath, string contentType, string contentId, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FilePath = filePath,
                ContenType = contentType,
                FileName = fileName,
                ContentId = contentId
            });
            return this;
        }

        /// <summary>
        /// Adds a linked resource (inline attachment) from a URL with a Content-ID.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="url">The URL to download the file from.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        public IAttachmentCollection AddLinkedResource(Uri url, string contentId, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FileUrl = url,
                FileName = fileName,
                ContentId = contentId
            });
            return this;
        }

        /// <summary>
        /// Adds a linked resource (inline attachment) from a URL with a Content-ID and content type.
        /// Reference in HTML using: &lt;img src="cid:{contentId}" /&gt;
        /// </summary>
        /// <param name="url">The URL to download the file from.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="contentId">The Content-ID to reference in HTML (without 'cid:' prefix).</param>
        /// <param name="fileName">Optional file name override.</param>
        public IAttachmentCollection AddLinkedResource(Uri url, string contentType, string contentId, string? fileName = null)
        {
            this.attachments.Add(new AttachmentModel()
            {
                FileUrl = url,
                ContenType = contentType,
                FileName = fileName,
                ContentId = contentId
            });
            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<AttachmentModel> GetEnumerator()
        {
            return this.attachments.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IList<AttachmentModel> ToList()
        {
            return this.attachments;
        }

        public bool IsEmpty()
        {
            return this.attachments.Count == 0;
        }
    }
}

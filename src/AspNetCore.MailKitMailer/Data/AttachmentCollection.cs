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

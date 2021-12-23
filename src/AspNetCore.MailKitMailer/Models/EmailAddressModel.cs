using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Models
{
    /// <summary>
    /// EmailAddressModel
    /// </summary>
    public class EmailAddressModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressModel"/> class.
        /// </summary>
        public EmailAddressModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        public EmailAddressModel(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
    }
}

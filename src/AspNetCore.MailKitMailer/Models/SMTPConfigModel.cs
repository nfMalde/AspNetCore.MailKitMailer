using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.MailKitMailer.Models
{
    /// <summary>
    /// SMTPConfigModel
    /// </summary>
    public class SMTPConfigModel
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string? Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; } = 25;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use SSL].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use SSL]; otherwise, <c>false</c>.
        /// </value>
        public bool UseSSL { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [check certificate revocation].
        /// </summary>
        /// <remarks>Im some cases this option is required cause MailKit cant check for cert revocation. Handle with care</remarks>
        /// <value>
        ///   <c>true</c> if [check certificate revocation]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckCertificateRevocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [do authenticate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do authenticate]; otherwise, <c>false</c>.
        /// </value>
        public bool DoAuthenticate { get; set; } = true;

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>
        /// From address.
        /// </value>
        public EmailAddressModel? FromAddress { get; set; }
    }
}

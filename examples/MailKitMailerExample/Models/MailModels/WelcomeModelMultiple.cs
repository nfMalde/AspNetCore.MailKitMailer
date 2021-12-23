using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailKitMailerExample.Models.MailModels
{
    public class WelcomeModelMultiple
    {
        public List<string> Usernames { get; set; } = new List<string>();
    }
}

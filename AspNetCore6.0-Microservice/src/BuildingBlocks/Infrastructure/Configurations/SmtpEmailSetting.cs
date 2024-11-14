using Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class SmtpEmailSetting : IEmailSMTPSetting
    {
        public string DisplayName { get; set; } = null!;
        public bool EnableVerification { get; set; }
        public string From { get; set; } = null!;
        public string SMTPServer { get; set; } = null!;
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

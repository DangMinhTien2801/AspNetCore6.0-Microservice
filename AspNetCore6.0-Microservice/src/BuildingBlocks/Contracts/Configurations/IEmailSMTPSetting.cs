using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Configurations
{
    public interface IEmailSMTPSetting
    {
        string DisplayName { get; set; }
        bool EnableVerification { get; set; }
        string From { get; set; }
        string SMTPServer { get; set; }
        bool UseSsl { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}

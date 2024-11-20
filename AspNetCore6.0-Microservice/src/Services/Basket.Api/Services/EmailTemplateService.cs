using Shared.Configurations;
using System.Text;

namespace Basket.Api.Services
{
    public class EmailTemplateService
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _tmplFolder = Path.Combine(_baseDirectory, "EmailTemplates");

        protected readonly BackgroundJobSettings BackgroundJobSettings;
        public EmailTemplateService(BackgroundJobSettings backgroundJobSettings)
        {
            BackgroundJobSettings = backgroundJobSettings;
        }

        protected string ReadEmailTemplateContent(string templateEmail, string format = "html")
        {
            var filePath = Path.Combine(_tmplFolder, templateEmail + "." + format);
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            var emailText = sr.ReadToEnd();
            sr.Close();

            return emailText;
        }
    }
}

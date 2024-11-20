using Basket.Api.Services.Interfaces;
using Shared.Configurations;

namespace Basket.Api.Services
{
    public class BasketEmailTemplateService : EmailTemplateService, IEmailTemplateService
    {
        public BasketEmailTemplateService(BackgroundJobSettings backgroundJobSettings) 
            : base(backgroundJobSettings)
        {
        }

        public string GenerateReminderCheckoutOrderEmail(string userName, string checkoutUrl = "basket/checkout")
        {
            var _checkoutUrl = $"{BackgroundJobSettings.ApiGwUrl}/{checkoutUrl}/{userName}";
            var emailText = ReadEmailTemplateContent("reminder-checkout-order");
            var emailReplacedText = emailText.Replace("[username]", userName)
                .Replace("[checkoutUrl]", _checkoutUrl);

            return emailReplacedText;
        }
    }
}

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

        public string GenerateReminderCheckoutOrderEmail(string userName)
        {
            var _checkoutUrl = $"{BackgroundJobSettings.CheckoutUrl}/{BackgroundJobSettings.BasketUrl}/{userName}";
            var emailText = ReadEmailTemplateContent("reminder-checkout-order");
            var emailReplacedText = emailText.Replace("[username]", userName)
                .Replace("[checkoutUrl]", _checkoutUrl);

            return emailReplacedText;
        }
    }
}

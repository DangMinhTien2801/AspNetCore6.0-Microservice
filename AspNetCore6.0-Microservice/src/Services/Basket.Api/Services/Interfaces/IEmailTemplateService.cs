namespace Basket.Api.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateReminderCheckoutOrderEmail(string userName, string checkoutUrl = "basket/checkout");
        //string GenerateConfirmedCheckoutOrderEmail(string email, string userName);
    }
}

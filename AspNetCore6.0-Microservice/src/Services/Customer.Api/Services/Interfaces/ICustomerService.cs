namespace Customer.Api.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUserNameAsync(string userName);
        Task<IResult> GetCustomersAsync();
    }
}

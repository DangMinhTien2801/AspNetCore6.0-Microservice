using Contracts.Common.Interfaces;
using Customer.Api.Persistence;

namespace Customer.Api.Reponsitories.Interfaces
{
    public interface ICustomerRepository 
        : IRepositoryBaseAsync<Entities.Customer, string, CustomerContext>
    {
        Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName);
        Task<IEnumerable<Entities.Customer>> GetCustomersAsync();
    }
}

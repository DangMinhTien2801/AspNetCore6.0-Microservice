using Contracts.Common.Interfaces;
using Customer.Api.Persistence;
using Customer.Api.Reponsitories.Interfaces;
using Infrastructure.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Customer.Api.Reponsitories
{
    public class CustomerRepository 
        : RepositoryBaseAsync<Entities.Customer, string, CustomerContext>,
        ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext, 
            IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName)
            => await FindByCondition(c => c.UserName == userName).FirstOrDefaultAsync();

        public async Task<IEnumerable<Entities.Customer>> GetCustomersAsync()
        {
            return await FindAll().ToListAsync();
        }
    }
}

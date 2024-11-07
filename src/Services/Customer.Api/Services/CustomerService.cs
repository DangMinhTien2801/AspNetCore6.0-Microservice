using Customer.Api.Reponsitories;
using Customer.Api.Reponsitories.Interfaces;
using Customer.Api.Services.Interfaces;

namespace Customer.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<IResult> GetCustomerByUserNameAsync(string userName)
            => Results.Ok(await _customerRepository.GetCustomerByUserNameAsync(userName));

        public async Task<IResult> GetCustomersAsync()
            => Results.Ok(await _customerRepository.GetCustomersAsync());
    }
}

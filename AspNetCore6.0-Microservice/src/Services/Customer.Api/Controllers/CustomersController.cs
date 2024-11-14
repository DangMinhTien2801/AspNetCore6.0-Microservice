using AutoMapper;
using Customer.Api.Reponsitories.Interfaces;
using Customer.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer;

namespace Customer.Api.Controllers
{
    public static class CustomersController
    {
        public static void MapCustomersAPI(this WebApplication app)
        {
            app.MapGet("/", () => "Webcome to Customer API");

            app.MapGet("/api/v1/customers", async (ICustomerService customerService) =>
            {
                return await customerService.GetCustomersAsync();
            });

            app.MapGet("/api/v1/customers/{username}", async (ICustomerService customerService,
                string username) =>
            {
                var customer = await customerService
                    .GetCustomerByUserNameAsync(username);
                return customer == null ? Results.NotFound() : Results.Ok(customer);
            });

            app.MapPost("/api/v1/customers", async (CreateCustomerDto customerDto,
                ICustomerRepository customerRepository, IMapper mapper) =>
            {
                var customer = mapper.Map<Customer.Api.Entities.Customer>(customerDto);
                customer.Id = Guid.NewGuid().ToString();
                await customerRepository.CreateAsync(customer);
                var result = await customerRepository.SaveChangeAsync();
                return result < 1 ? Results.BadRequest(result) : Results.Ok(result);
            });

            app.MapPut("/api/v1/customers/{customerId}", async (string customerId, [FromBody] UpdateCustomerDto customerDto,
                ICustomerRepository customerRepository, IMapper mapper) =>
            {
                var customer = await customerRepository.GetByIdAsync(customerId);
                if (customer == null)
                    return Results.NotFound();
                var customerUpdate = mapper.Map(customerDto, customer);
                await customerRepository.UpdateAsync(customerUpdate);
                var result = await customerRepository.SaveChangeAsync();
                return result < 1 ? Results.BadRequest(result) : Results.Ok(result);
            });

            app.MapDelete("/api/v1/customers/{customerId}", async (string customerId,
                ICustomerRepository customerRepository) =>
            {
                var customer = await customerRepository.GetByIdAsync(customerId);
                if (customer == null)
                    return Results.NotFound();
                await customerRepository.DeleteAsync(customer);
                var result = await customerRepository.SaveChangeAsync();
                return result < 1 ? Results.BadRequest(result) : Results.Ok(result);
            });
        }
    }
}

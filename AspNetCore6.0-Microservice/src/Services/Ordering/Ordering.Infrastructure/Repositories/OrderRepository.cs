using Contracts.Common.Interfaces;
using Infrastructure.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Odering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository 
        : RepositoryBaseAsync<Order, string, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) 
            : base(dbContext, unitOfWork)
        {
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await CreateAsync(order);
            await SaveChangeAsync();
            return order;
        }

        public async Task<Order?> GetOrderByDocumentNo(string documentNo)
        {
            return await FindByCondition(o => o.DocumentNo.ToString() == documentNo)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            return await FindByCondition(o => o.UserName == userName)
                .ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            await UpdateAsync(order);
            await SaveChangeAsync();
            return order;
        }
    }
}

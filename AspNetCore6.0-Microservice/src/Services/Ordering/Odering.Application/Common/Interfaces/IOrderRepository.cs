using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepositoryBaseAsync<Order, string>
    {
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);
        Task<Order?> GetOrderByDocumentNo(string documentNo);
        Task<Order> CreateOrder(Order order);
        Task<Order> UpdateOrderAsync(Order order);
    }
}

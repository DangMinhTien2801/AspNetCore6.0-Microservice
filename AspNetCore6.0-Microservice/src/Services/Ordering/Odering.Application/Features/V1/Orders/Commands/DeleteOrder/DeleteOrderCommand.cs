using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public DeleteOrderCommand(string id)
        {
            Id = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Enums
{
    public enum EOrderStatus
    {
        New = 1,
        Pending = 2,
        Paid = 3,
        Shipping = 4,
        Fulfilled = 5,
    }
}

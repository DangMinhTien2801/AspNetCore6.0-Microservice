using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class SalesOrderDto
    {
        // Order document No
        public string OrderNo { get; set; }
        public List<SaleItemDto> SaleItems { get; set; }
    }
}

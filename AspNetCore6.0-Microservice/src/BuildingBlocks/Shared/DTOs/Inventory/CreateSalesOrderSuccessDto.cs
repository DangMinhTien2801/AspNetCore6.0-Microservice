using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class CreateSalesOrderSuccessDto
    {
        public string DocumentNo { get; set; }
        public CreateSalesOrderSuccessDto(string documentNo)
        {
            DocumentNo = documentNo;
        }
    }
}

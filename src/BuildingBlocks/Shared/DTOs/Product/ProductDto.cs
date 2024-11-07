using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Product
{
    public class ProductDto
    {
        public string Id { get; set; } = null!;
        public string No { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}

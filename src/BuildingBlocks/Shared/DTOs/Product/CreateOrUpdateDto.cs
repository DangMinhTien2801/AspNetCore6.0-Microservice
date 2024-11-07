using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Product
{
    public abstract class CreateOrUpdateDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        [MaxLength(250, ErrorMessage = "Maxium length for Product Name is {1}")]
        public string Name { get; set; } = null!;
        [MaxLength(250, ErrorMessage = "Maxium length for Product Name is {1}")]
        public string? Summary { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Product Price is required")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Product Price must from {1} to {2}")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Product Stock Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Product Stock Quantity must from {1} to {2}")]
        public int StockQuantity { get; set; }
    }
}

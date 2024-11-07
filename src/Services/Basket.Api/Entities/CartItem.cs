using System.ComponentModel.DataAnnotations;

namespace Basket.Api.Entities
{
    public class CartItem
    {
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Item Price is required")]
        [Range(0.1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}")]
        public decimal ItemPrice { get; set; }
        public string ItemNo { get; set; } = null!;
        public string ItemName { get; set; } = null!;
    }
}

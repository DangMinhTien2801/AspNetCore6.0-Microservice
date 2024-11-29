using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Basket
{
    public class CartDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public List<CartItemDto> Items { get; set; }
        public CartDto() { }

        public CartDto(string userName)
        {
            UserName = userName;    
        }

        public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Customer
{
    public class CreateCustomerDto : UpdateOrCreateDto
    {
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "The field EmailAddress must be email address")]
        public string EmailAddress { get; set; } = null!;
    }
}

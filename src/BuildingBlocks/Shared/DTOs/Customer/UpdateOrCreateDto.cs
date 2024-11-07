using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Customer
{
    public class UpdateOrCreateDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(100, ErrorMessage = "Maxium for length First Name is {1}")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(150, ErrorMessage = "Maxium for length Last Name is {1}")]
        public string LastName { get; set; } = null!;
    }
}

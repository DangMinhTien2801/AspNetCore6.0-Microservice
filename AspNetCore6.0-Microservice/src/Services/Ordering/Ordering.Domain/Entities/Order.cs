using Contracts.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order : EntityAuditBase<string>
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string FirstName { get; set; } = null!;
        public Guid DocumentNo { get; set; } = Guid.NewGuid();
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string LastName { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string EmailAddress { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string ShippingAddress { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string InvoiceAddress { get; set; } = null!;
        [Required]
        public int Status {  get; set; }
    }
}

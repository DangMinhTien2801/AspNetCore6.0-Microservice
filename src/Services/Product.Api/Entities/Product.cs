using Contracts.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Api.Entities
{
    public class CatalogProduct : EntityAuditBase<string>
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string No { get; set; } = null!;
        [Required]
        [DataType("text")]
        public string Name { get; set; } = null!;
        [DataType("nvarchar(max)")]
        public string? Summary { get; set; }
        [DataType("text")]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}

using Contracts.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.Api.Entities
{
    public class Customer : EntityBase<string>
    {
        public string UserName { get; set; } = null!;
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; } = null!;
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; } = null!;
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Email
{
    public class MailRequest
    {
        [Required(ErrorMessage = "From is required")]
        [EmailAddress(ErrorMessage = "From must be a email")]
        public string From { get; set; } = null!;
        [Required(ErrorMessage = "ToAddress is required")]
        [EmailAddress(ErrorMessage = "ToAddress must be a email")]
        public string ToAddress { get; set; } = null!;
        public IEnumerable<string> ToAddresses { get; set;} = new List<string>();
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = null!;
        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = null!;
        public IFormFileCollection? Attachments { get; set; }
    }
}

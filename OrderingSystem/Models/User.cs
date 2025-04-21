using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(11)]
        public string ContactNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
    }
}

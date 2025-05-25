using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        //[Column(TypeName = "decimal(18,4)")]
        //public decimal Price { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}

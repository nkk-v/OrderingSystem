using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant productVariant { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        //[Column(TypeName = "decimal(18,4)")]
        // public decimal Price => Product.Price;
    }
}

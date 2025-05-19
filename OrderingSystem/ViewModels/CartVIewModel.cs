using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.ViewModels
{
    public class CartVIewModel
    {
      
        public List<CartItemViewModel> CartItems { get; set; } = new();
        [Column(TypeName = "decimal(18,4)")]
        public decimal Total { get; set; }
        public bool HasItems => CartItems != null && CartItems.Any();
    }
}

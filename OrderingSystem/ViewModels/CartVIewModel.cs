namespace OrderingSystem.ViewModels
{
    public class CartVIewModel
    {
      
        public List<CartItemViewModel> CartItems { get; set; } = new();
        public double Total { get; set; }
        public bool HasItems => CartItems != null && CartItems.Any();
    }
}

using OrderingSystem.Models;

namespace OrderingSystem.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price => Product.Price;
    }
}

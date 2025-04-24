using OrderingSystem.Models;

namespace OrderingSystem.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}

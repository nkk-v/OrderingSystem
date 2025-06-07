namespace OrderingSystem.ViewModels
{
    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

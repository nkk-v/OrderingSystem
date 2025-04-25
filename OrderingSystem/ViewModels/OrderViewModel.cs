namespace OrderingSystem.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid OrderNum { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Fullname { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string? DeliveryNote { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}

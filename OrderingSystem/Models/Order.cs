using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        [StringLength(15)]
        public string Status { get; set; } // Example: Pending, Shipped, Delivered
        public string DeliverySchedule { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

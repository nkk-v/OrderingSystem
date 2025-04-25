using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Guid OrderNum { get; set; }
        public string UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public double TotalAmount { get; set; }
        [StringLength(15)]
        public string Status { get; set; } // Example: Pending, Shipped, Delivered
        public DateTime? ScheduledDate { get; set; }
        public string? DeliveryNote { get; set; }
        [StringLength(50)]
        public string fullname { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

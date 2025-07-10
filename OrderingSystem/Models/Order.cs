using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNum { get; set; }
        public string UserId { get; set; }
        public DateOnly? OrderDate { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal SubTotal { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal DeliveryFee { get; set; }
        [StringLength(15)]
        public string DeliveryStatus { get; set; } // Example: Pending, Shipped, Delivered
        public DateTime? ScheduledTimeStart { get; set; }
        public DateTime? ScheduledTimeEnd { get; set; }
        public string? DeliveryNote { get; set; }
        [StringLength(50)]
        public string fullname { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        [StringLength(10)]
        public string OrderStatus { get; set; } // Example: Pending, Success, Failed, Expired
        [StringLength(50)]
        public string? RefNo { get; set; } //From PayMongo payment id
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

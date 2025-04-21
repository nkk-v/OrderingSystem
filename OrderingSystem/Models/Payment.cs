using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [StringLength(10)]
        public string PaymentStatus { get; set; } // Pending, Success, Failed[Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; } // e.g., GCash, PayMaya
        public double Amount { get; set; }
    }
}

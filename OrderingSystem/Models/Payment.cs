using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

using OrderingSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.ViewModels
{
    public class CheckoutViewModel
    {
        public string? OrderNum { get; set; }
        [Required]
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string ManualAddress { get; set; }
        [Required]
        public string CurrentAddress { get; set; }
        [Required]
        public string DeliveryOption { get; set; }
        [Required]
        public DateTime? ScheduledDate { get; set; }
        [Required]
        public string? ScheduledTime { get; set; }
        public string? DeliveryNote { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal SubTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount => SubTotal + DeliveryFee;
    }
}

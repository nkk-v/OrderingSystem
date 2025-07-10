using OrderingSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.ViewModels
{
    public class CheckoutViewModel
    {
        public string? OrderNum { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
        public string ManualAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string DeliveryOption { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public DateTime? ScheduledTimeStart { get; set; }
        public DateTime? ScheduledTimeEnd { get; set; }
        public string? DeliveryNote { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal SubTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount => SubTotal + DeliveryFee;
        public int ItemCount { get; set; }
    }
}

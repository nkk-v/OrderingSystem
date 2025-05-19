using OrderingSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.ViewModels
{
    public class CheckoutViewModel
    {
        public string? OrderNum { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string DeliveryOption { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? ScheduledTime { get; set; }
        public string? DeliveryNote { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmout => CartItems.Sum(x => x.Price * x.Quantity);
    }
}

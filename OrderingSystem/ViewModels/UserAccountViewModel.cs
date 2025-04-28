using OrderingSystem.Models;

namespace OrderingSystem.ViewModels
{
    public class UserAccountViewModel
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
        public bool HasItems => Orders != null && Orders.Any();
    }
}

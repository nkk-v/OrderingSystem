using EnvDTE;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderingSystem.ViewModels
{
    public class OrderFilterViewModel
    {
        public string OrderStatus { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
    }
}

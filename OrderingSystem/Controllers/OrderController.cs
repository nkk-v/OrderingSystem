using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrders();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int OrderId, string newStatus)
        {

            await _orderService.UpdateOrderStatus(OrderId, newStatus);
            TempData["Success"] = "Order status updated successfully.";

            return RedirectToAction("Index");
        }
    }
}

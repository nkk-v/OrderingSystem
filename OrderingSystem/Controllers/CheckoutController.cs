using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Models;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;
using System.Security.Claims;

namespace OrderingSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;
        private readonly ICartService _cartService;

        public CheckoutController(ICheckoutService checkoutService, IOrderService orderService, UserManager<User> userManager, ICartService cartService)
        {
            _checkoutService = checkoutService;
            _orderService = orderService;
            _userManager = userManager;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await _checkoutService.GetCheckoutViewModelAsync(userId);

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User);


            string OrderNumber = await _orderService.AddOrder(model, userId);
            await _cartService.ClearCartItems(userId);

           
            return RedirectToAction("Success", new {orderNum = OrderNumber});
        }

        public IActionResult Success(string orderNum)
        {
            ViewBag.orderNum = orderNum;
            return View();
        }
    }
}

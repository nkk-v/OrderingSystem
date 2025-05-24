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
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreatePayment(CheckoutViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var checkoutUrl = await _checkoutService.CreatePaymentLinkAsync(model, userId);
            return Redirect(checkoutUrl);
            //if (!ModelState.IsValid) return View(model);

            //var userId = _userManager.GetUserId(User);



            //string OrderNumber = await _orderService.AddOrder(model, userId);
            //await _cartService.ClearCartItems(userId);

           
            //return RedirectToAction("Success", new {orderNum = OrderNumber});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Success()
        {
            var userId = _userManager.GetUserId(User);

            string orderNum = await _orderService.GetLatestOrderByUser(userId);

            await _cartService.ClearCartItems(userId);

            ViewBag.orderNum = orderNum;
            return View();
        }

        public async Task<IActionResult> Failed()
        {
            var userId = _userManager.GetUserId(User);

            var orderId = await _orderService.GetLatestOrderId("", userId);

            await _orderService.UpdateOrderStatus(orderId, "payment.failed", "");

            return View();
        }
    }
}

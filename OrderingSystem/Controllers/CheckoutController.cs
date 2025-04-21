using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Services;
using System.Security.Claims;

namespace OrderingSystem.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await _checkoutService.GetCheckoutViewModelAsync(userId);   

            return View(model);
        }
    }
}

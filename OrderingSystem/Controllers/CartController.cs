using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Models;
using OrderingSystem.Services;

namespace OrderingSystem.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;

        public CartController(ICartService cartService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetUserId();
            if (userId == null) return RedirectToAction("login", "Account");
            var cart = await _cartService.GetUserCart(userId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = await GetUserId();
            if (userId == null) return RedirectToAction("login", "Account");

            await _cartService.AddtoCart(userId, productId);
            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _cartService.UpdateCart(cartItemId, quantity);
            return Ok();
        }

        public async Task<IActionResult> Remove(int cartItemId)
        {
            await _cartService.RemoveItem(cartItemId);
            return RedirectToAction("Index", "Cart");
        }
    }
}

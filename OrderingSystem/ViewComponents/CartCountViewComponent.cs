using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using OrderingSystem.Services;

namespace OrderingSystem.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartCountViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? userId = HttpContext.User.Identity.IsAuthenticated
            ? HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            : null;

            int count = userId != null
                ? await _cartService.GetItemCount(userId)
                : 0;

            return View(count);
        }
    }
}

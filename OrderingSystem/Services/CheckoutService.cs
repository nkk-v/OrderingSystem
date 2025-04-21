using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;

        public CheckoutService(ICartService cartService, IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }

        public async Task<CheckoutViewModel> GetCheckoutViewModelAsync(string userId)
        {
            var user = await _accountService.GetUserDetails(userId);
            var cart = await _cartService.GetUserCart(userId);

            return new CheckoutViewModel
            {
                Fullname = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CartItems = cart.CartItems
            };
        }
    }
}

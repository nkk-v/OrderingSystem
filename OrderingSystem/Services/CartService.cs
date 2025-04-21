using OrderingSystem.Models;
using OrderingSystem.Repositories;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;

        public CartService(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task AddtoCart(string userId, int productId, int quantity = 1)
        {
             await _cartRepo.AddtoCart(userId, productId, quantity); 
        }

        public async Task<int> GetItemCount(string userId)
        {
            return await _cartRepo.GetCartItemCountByUser(userId);
        }

        public async Task<CartVIewModel> GetUserCart(string userId)
        {
            var cart = await _cartRepo.GetUserCart(userId);

            if (cart == null) return new CartVIewModel { CartItems = new List<CartItemViewModel>(), Total = 0};

            var viewModel = new CartVIewModel
            {
                CartItems = cart.CartItems.Select(item => new CartItemViewModel
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    ProductId = item.ProductId,
                    Cart = item.Cart,
                    Product = item.Product,
                    Quantity = item.Quantity

                }).ToList(),
                Total = cart.CartItems.Sum(x => x.Price * x.Quantity)
            };

            return viewModel;
        }

        public async Task RemoveItem(int cartItemId)
        {
            await _cartRepo.RemoveItem(cartItemId);
        }

        public async Task UpdateCart(int cartItemId, int quantity)
        {
            await _cartRepo.UpdateCart(cartItemId, quantity);
        }
    }
}

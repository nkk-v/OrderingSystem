using OrderingSystem.Models;
using OrderingSystem.Repositories;

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

        public async Task<Cart> GetUserCart(string userId)
        {
            return await _cartRepo.GetUserCart(userId);
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

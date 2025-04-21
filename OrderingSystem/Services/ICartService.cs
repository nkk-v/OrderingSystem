using OrderingSystem.Models;

namespace OrderingSystem.Services
{
    public interface ICartService
    {
        Task<Cart> GetUserCart(string userId);
        Task AddtoCart(string userId, int productId, int quantity = 1);
        Task UpdateCart(int cartItemId, int quantity);
        Task RemoveItem(int cartItemId);
        Task<int> GetItemCount(string userId);
    }
}

using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface ICartService
    {
        Task<CartVIewModel> GetUserCart(string userId);
        Task AddtoCart(string userId, int productId, int quantity = 1);
        Task UpdateCart(int cartItemId, int quantity);
        Task RemoveItem(int cartItemId);
        Task<int> GetItemCount(string userId);
        Task ClearCartItems(string userId);
    }
}

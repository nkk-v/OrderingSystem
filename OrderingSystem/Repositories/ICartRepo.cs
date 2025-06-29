using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public interface ICartRepo
    {
        Task<Cart> GetUserCart(string userId);
        Task AddtoCart(string userId, int productId, int variantId, int quantity = 1);
        Task UpdateCart(int cartItemId, int quantity);
        Task RemoveItem(int cartItemId);
        Task<int> GetCartItemCountByUser(string userId);
        Task ClearCartItem(string userId);
        Task RemoveCartItemsByVariantIdsAsync(List<int> variantIds);

    }
}

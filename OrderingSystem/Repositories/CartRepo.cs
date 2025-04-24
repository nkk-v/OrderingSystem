using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _dbContext;

        public CartRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<Cart> GetUserCart(string userId)
        {
            var cart = await _dbContext.tblCarts
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
            {
                cart = new Cart { UserId = userId};
                _dbContext.tblCarts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddtoCart(string userId, int productId, int quantity = 1)
        {
            var cart = await GetUserCart(userId);

            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task RemoveItem(int cartItemId)
        {
            var item = await _dbContext.tblCartItems.FindAsync(cartItemId);
            if(item != null)
            {
                _dbContext.tblCartItems.Remove(item);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateCart(int cartItemId, int quantity)
        {
            var item = await _dbContext.tblCartItems.FindAsync(cartItemId);
            if(item != null)
            {
                item.Quantity = quantity;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> GetCartItemCountByUser(string userId)
        {
            
            var cart = await _dbContext.tblCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            if(cart == null) return 0;
            
            return await _dbContext.tblCartItems.Where(
                x => x.CartId == cart.Id
                ).CountAsync();
        }

        public async Task ClearCartItem(string userId)
        {
            var cart = await _dbContext.tblCarts
                 .Include(c => c.CartItems)
                 .ThenInclude(p => p.Product)
                 .FirstOrDefaultAsync(c => c.UserId == userId);

            var cartItems = await _dbContext.tblCartItems.Where(x => x.CartId == cart.Id).ToListAsync();

            _dbContext.tblCartItems.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();

        }
    }
}

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
                .Include(c => c.CartItems.Where(ci => ci.IsActive))
                    .ThenInclude(pv => pv.productVariant)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
            {
                cart = new Cart { UserId = userId};
                _dbContext.tblCarts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddtoCart(string userId, int productId, int variantId, int quantity = 1)
        {
            var cart = await GetUserCart(userId);

            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId && x.ProductVariantId == variantId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var variant = await _dbContext.tblProductVariant.FirstOrDefaultAsync(x => x.Id == variantId && x.ProductId == productId); 
                if(variant == null) throw new Exception("Variant not found or does not match product.");


                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    ProductVariantId = variantId,
                    Quantity = quantity,
                    Price = variant.Price,
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
            
            var cart = await _dbContext.tblCarts.FirstOrDefaultAsync(x => x.UserId == userId );
            if(cart == null) return 0;
            
            return await _dbContext.tblCartItems.Where(
                x => x.CartId == cart.Id && x.IsActive).CountAsync();
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

        public async Task RemoveCartItemsByVariantIdsAsync(List<int> variantIds)
        {
            if (variantIds == null || variantIds.Count == 0) return;

            var cartItems = await _dbContext.tblCartItems
                            .Where(ci => variantIds.Contains(ci.ProductVariantId))
                            .ToListAsync();

            if (cartItems.Any())
            {
                //_dbContext.tblCartItems.RemoveRange(cartItems);
                foreach (var item in cartItems)
                {
                    item.IsActive = false;
                }
                await _dbContext.SaveChangesAsync();
            }
            
        }
    }
}

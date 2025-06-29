using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _dbContext;

        public ProductRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProduct(Product product)
        {
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var result = await _dbContext.tblProducts.FindAsync(id);
            if (result != null)
            {
                _dbContext.tblProducts.Remove(result);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.tblProducts.ToListAsync();
        }

        public async Task<List<Product>> GetByCategory(int? categoryId)
        {
            var query = _dbContext.tblProducts.Include(x => x.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId && x.IsActive);
            }
            else
            {
                query = query.Where(x => x.IsActive);
            }

            return await query.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.tblProducts
                .Include(p => p.Variants.Where(v => v.IsActive))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task RemoveProductVariant(int productId)
        {
            var product = await _dbContext.tblProducts
                .Include(v => v.Variants)
                .FirstOrDefaultAsync(v => v.Id == productId);

            if (product == null) return;

            _dbContext.tblProductVariant.RemoveRange(product.Variants);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveProductVariantsByIdsAsync(List<int> variantIds)
        {

            if (variantIds == null || !variantIds.Any()) return;

            var variants = await _dbContext.tblProductVariant
                            .Where(v => variantIds.Contains(v.Id))
                            .ToListAsync();

            //_dbContext.tblProductVariant.RemoveRange(variants);

            foreach ( var variant in variants)
            {
                variant.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _dbContext.tblProducts.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}

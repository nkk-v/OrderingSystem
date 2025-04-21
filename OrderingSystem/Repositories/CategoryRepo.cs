using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCategory(Category category)
        {
            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var result = await _dbContext.tblCategories.FindAsync(id);
            if (result != null)
            {
                _dbContext.tblCategories.Remove(result);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbContext.tblCategories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbContext.tblCategories.FindAsync(id);
        }

        //public async Task<IEnumerable<Category>> searchCategory(string searchTerm)
        //{
        //    if (string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        return await _dbContext.tblCategories.ToListAsync();
        //    }

        //    return await _dbContext.tblCategories.Where(x => x.Name.Contains(searchTerm)).OrderBy(x => x.Id).ToListAsync();   
        //}

        public async Task UpdateCategory(Category category)
        {
            _dbContext.tblCategories.Update(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}

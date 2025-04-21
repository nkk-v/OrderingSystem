using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
       // Task<IEnumerable<Category>> searchCategory(string searchTerm);
    }
}

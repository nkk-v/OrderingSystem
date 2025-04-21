using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllAsync(string? searchTerm);
        Task<CategoryViewModel> GetByIdAsync(int id);
        Task AddCategory(CategoryViewModel model, string uploadRootPath);
        Task<bool> DeleteCategory(int id, string uploadRootPath);
        Task<bool> UpdateCategory(CategoryViewModel model, string uploadRootPath);
        Task<List<Category>> GetCategoriesAsync();
        //Task<IEnumerable<Category>> SearchCategory(string searchTerm);
    }
}

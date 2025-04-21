using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetAllAsync(string? searchTerm);
        Task<ProductViewModel> GetByIdAsync(int id);
        Task AddProduct(ProductViewModel model, string uploadRootPath);
        Task<bool> DeleteProduct(int id, string uploadRootPath);
        Task<bool> UpdateProduct(ProductViewModel model, string uploadRootPath);    
        Task<ProductViewModel>PopulateCategory(ProductViewModel model);
        Task<ProductViewModel> PrepareProductCatagory();
        Task<List<Product>> GetProductsAsync(int? categoryId);
    }
}

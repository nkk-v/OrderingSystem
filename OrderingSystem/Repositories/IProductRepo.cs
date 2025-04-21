using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public interface IProductRepo
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);

        Task<List<Product>> GetByCategory(int? categoryId);
    }
}

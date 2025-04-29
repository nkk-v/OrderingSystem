using EnvDTE;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderingSystem.Models;
using OrderingSystem.Repositories;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;

        public ProductService(IProductRepo productRepo, ICategoryRepo categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task AddProduct(ProductViewModel model, string uploadRootPath)
        {

            string? imagePath = null;
            if(model.ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                string path = Path.Combine(uploadRootPath, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                imagePath = "/uploads/product/" + fileName;
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = imagePath,
                CategoryId = model.CategoryId,
                IsActive = model.IsActive,
                DateCreated = DateTime.Now
            };

            await _productRepo.AddProduct(product);
            
        }

        public async Task<bool> DeleteProduct(int id, string uploadRootPath)
        {
           var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return false;

            var result = await _productRepo.DeleteProduct(id);
            if(!result) return false;

            //delete old image
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string oldpath = Path.Combine(uploadRootPath, Path.GetFileName(product.ImageUrl));
                if (File.Exists(oldpath))
                    File.Delete(oldpath);
            }

            return result;
        }

        public async Task<List<ProductViewModel>> GetAllAsync(string? searchTerm)
        {
            var product = await _productRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                product = product.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) && x.IsActive).OrderBy(
                    x => x.Id).ToList();
            }
            
            var categories = await _categoryRepo.GetAllAsync();

            return product.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CategoryName = categories.FirstOrDefault(c => c.Id == x.CategoryId)?.Name
            }).ToList();
        }

        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return null;

            var categories = await _categoryRepo.GetAllAsync();

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };
        }

        public async Task<List<Product>> GetProductsAsync(int? categoryId)
        {
            return await _productRepo.GetByCategory(categoryId);
        }

        public async Task<ProductViewModel> PopulateCategory(ProductViewModel model)
        {
            var categories = await _categoryRepo.GetAllAsync();

            model.Categories = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return model;
        }

        public async Task<ProductViewModel> PrepareProductCatagory()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return new ProductViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };
        }

        public async Task<bool> UpdateProduct(ProductViewModel model, string uploadRootPath)
        {
            var product = await _productRepo.GetByIdAsync(model.Id);
            if (product == null) return false;

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.IsActive = model.IsActive;

            if (model.ImageFile != null)
            {
                //delete old image
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string oldpath = Path.Combine(uploadRootPath, Path.GetFileName(product.ImageUrl));
                    if (File.Exists(oldpath))
                        File.Delete(oldpath);
                }

                //save new image
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                string newpath = Path.Combine(uploadRootPath, fileName);

                using var stream = new FileStream(newpath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                product.ImageUrl = "/uploads/product/" + fileName;
            }

             await _productRepo.UpdateProduct(product);
            return true;
        }
    }
}

using OrderingSystem.Models;
using OrderingSystem.Repositories;
using OrderingSystem.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace OrderingSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task AddCategory(CategoryViewModel model, string uploadRootPath)
        {
            string? imagePath = null;
            if (model.ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                string path = Path.Combine(uploadRootPath, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                imagePath = "/uploads/category/" + fileName;
            }

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = imagePath

            };

            await _categoryRepo.AddCategory(category);
        }

        public async Task<bool> DeleteCategory(int id, string uploadRootPath)
        {

            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return false;

            var result = await _categoryRepo.DeleteCategory(id);

            if (!result) return false;

            //delete old image
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                string oldpath = Path.Combine(uploadRootPath, Path.GetFileName(category.ImageUrl));
                if (File.Exists(oldpath))
                    File.Delete(oldpath);
            }

            
            return result;
        }

        public async Task<List<CategoryViewModel>> GetAllAsync(string? searchTerm)
        {
            var categories = await _categoryRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                categories = categories.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).OrderBy(x => x.Id).ToList();
            }

            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl
            }).ToList();
        }

        public async Task<CategoryViewModel?> GetByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
            };
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepo.GetAllAsync();
        }

        //public async Task<IEnumerable<Category>> SearchCategory(string searchTerm)
        //{
        //    return await _categoryRepo.searchCategory(searchTerm);
        //}

        public async Task<bool> UpdateCategory(CategoryViewModel model, string uploadRootPath)
        {
            var category = await _categoryRepo.GetByIdAsync(model.Id);
            if (category == null) return false;

            category.Name = model.Name;
            category.Description = model.Description;


            if (model.ImageFile != null)
            {
                //delete old image
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    string oldpath = Path.Combine(uploadRootPath, Path.GetFileName(category.ImageUrl));
                    if (File.Exists(oldpath))
                        File.Delete(oldpath);
                }

                //save new image
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                string newpath = Path.Combine(uploadRootPath, fileName);

                using var stream = new FileStream(newpath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                category.ImageUrl = "/uploads/category/" + fileName;
            }

            

            await _categoryRepo.UpdateCategory(category);

            return true;
        }


    }
}

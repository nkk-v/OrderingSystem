using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment env)
        {
            _categoryService = categoryService;
            _env = env;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var category = await _categoryService.GetAllAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(category);
        }

        public IActionResult Create() 
        {
            ViewData["Action"] = "Create";
            return PartialView("_CategoryForm", new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {

            if(!ModelState.IsValid)
            {
                return PartialView("_CategoryForm", model);
            }
            string uploadRootPath = Path.Combine(_env.WebRootPath, "uploads\\category");
            await _categoryService.AddCategory(model, uploadRootPath);
            return Ok();
        }

        public async Task<IActionResult> Edit(int id)
        {
           var category = await _categoryService.GetByIdAsync(id);
            ViewData["Action"] = "Edit";
            return PartialView("_CategoryForm", category);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_CategoryForm", model);
            }
            string uploadRootPath = Path.Combine(_env.WebRootPath, "uploads\\category");
            await _categoryService.UpdateCategory(model, uploadRootPath);
            return Ok();
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            string uploadRootPath = Path.Combine(_env.WebRootPath, "uploads\\category");
            await _categoryService.DeleteCategory(id, uploadRootPath);
            return RedirectToAction("Index","Category");
        }

    }
}

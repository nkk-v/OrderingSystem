using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var product = await _productService.GetAllAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _productService.PrepareProductCatagory();
            ViewData["Action"] = "Create";
            return PartialView("_ProductForm", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_ProductForm", model);
            }

            string uploadRootPath = Path.Combine(_env.WebRootPath, "uploads\\product");
            await _productService.AddProduct(model, uploadRootPath);
            return Ok();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            ViewData["Action"] = "Edit";
            return PartialView("_ProductForm", product);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ProductForm", model);
            }

            string uploadRootPath = Path.Combine(_env.WebRootPath, "uploads\\product");
            await _productService.UpdateProduct(model, uploadRootPath);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string uploadRootPath = Path.Combine(_env.WebRootPath, "uploads\\product");
            await _productService.DeleteProduct(id, uploadRootPath);
            return RedirectToAction("Index", "Product");
        }
    }
}

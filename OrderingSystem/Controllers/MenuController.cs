using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Models;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Controllers
{
    public class MenuController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public MenuController(ICategoryService categoryService, IProductService productService, ICartService cartService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var model = new ProductCategoryViewModel
            {
                categories = await _categoryService.GetCategoriesAsync(),
                products = await _productService.GetProductsAsync(categoryId),
                SelectedCategoryId = categoryId
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductVariants(int productId)
        {
            var product = await _productService.GetByIdWithVariantsAsync(productId); // You may implement this if not available
            if (product == null) return NotFound();

            return Json(product.Variants.Select(v => new
            {
                id = v.Id,
                variantName = v.VariantName,
                price = v.Price
            }));
        }




    }
}

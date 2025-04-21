using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public OrderController(ICategoryService categoryService, IProductService productService, ICartService cartService)
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

        
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Models;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;
using System.Diagnostics;

namespace OrderingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICategoryService categoryService, IProductService productService, ICartService cartService, ILogger<HomeController> logger)
        {
            _logger = logger;
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

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

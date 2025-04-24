using Microsoft.AspNetCore.Mvc;

namespace OrderingSystem.Controllers
{
    public class OrderListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

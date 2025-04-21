using Microsoft.AspNetCore.Mvc;

namespace OrderingSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

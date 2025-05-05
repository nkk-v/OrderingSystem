using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Models;
using OrderingSystem.Services;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccountService accountService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _accountService.GetAllUserAndRoles();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRoles(string userId, string newRole)
        {
            var success = await _accountService.ChangeUserRole(userId, newRole);
            if (!success) return View();

            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> login(LoginViewModel model)
        {

           if (ModelState.IsValid)
            {
               var success = await _accountService.Login(model);
                if (!success)
                {
                    ViewData["error"] = "Username or Password is incorrect.";
                    return View(model);
                }

                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    if(await _userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }else if (await _userManager.IsInRoleAsync(user, "Customer"))
                    {
                        return RedirectToAction("Index", "Menu");
                    }

                    return View(model);
                }
            }
           return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _accountService.RegisterAccount(model);

                if (success)
                {
                    //ViewData["Message"] = "Account register successfully. ";
                    //return View(model);
                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", "Menu");
                }
                else
                {
                    ViewData["Message"] = "Registration failed.";
                    return View(model);
                }

            }

            return View(model);

        }

        public async Task<IActionResult> logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = await _accountService.GetUserDetails(user.Id);
            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var success = await _accountService.ChangePassword(userId, model);

                if (success)
                {
                    await _accountService.Logout();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
           
        }
    }
}

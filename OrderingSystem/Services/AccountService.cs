using Microsoft.AspNetCore.Identity;
using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> ChangePassword(string userId, ChangePasswordViewModel model)
        {
           var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> ChangeUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if(!await _roleManager.RoleExistsAsync(newRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(newRole));
            }

            var result = await _userManager.AddToRoleAsync(user, newRole);
            return result.Succeeded;
        }


        public async Task<List<UserRoleViewModel>> GetAllUserAndRoles()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return model;
        }

        public async Task<UserAccountViewModel> GetUserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new UserAccountViewModel
            {
                Fullname = user.FullName,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            }; 
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterAccount(RegisterViewModel model)
        {
            User user = new User
            {
                FullName = model.Fullname,
                UserName = model.Username,
                Address = model.Address,
                ContactNumber = model.ContactNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            return result.Succeeded;
        }
    }
}

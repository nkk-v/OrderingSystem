using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterAccount(RegisterViewModel model);
        Task<bool> Login(LoginViewModel model);
        Task Logout();
        Task<bool> ChangePassword(string userId, ChangePasswordViewModel model);
        Task<List<UserRoleViewModel>> GetAllUserAndRoles();
        Task<bool> ChangeUserRole(string userId, string newRole);
        Task<UserAccountViewModel> GetUserDetails(string userId);
        
    }
}

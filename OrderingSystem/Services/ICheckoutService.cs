using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutViewModel> GetCheckoutViewModelAsync(string userId);
        Task<string> CreatePaymentLinkAsync(CheckoutViewModel model, string userId);
    }
}

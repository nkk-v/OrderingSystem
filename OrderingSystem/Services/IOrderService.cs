using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface IOrderService
    {
        Task<int> AddOrder(CheckoutViewModel model, string userId);

    }
}

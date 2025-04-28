using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface IOrderService
    {
        Task<string> AddOrder(CheckoutViewModel model, string userId);
        Task<List<OrderViewModel>> GetAllOrders(string status);
        Task UpdateOrderStatus(int OrderId, string newStatus);
        Task<string> GenerateOrderNumber();
       
    }
}

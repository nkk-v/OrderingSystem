using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface IOrderService
    {
        Task<int> AddOrder(CheckoutViewModel model, string userId);
        Task<List<OrderViewModel>> GetAllOrders();
        Task UpdateOrderStatus(int OrderId, string newStatus);
       
    }
}

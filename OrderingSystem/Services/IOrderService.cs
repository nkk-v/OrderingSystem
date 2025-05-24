using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public interface IOrderService
    {
        Task<int> AddOrder(CheckoutViewModel model, string userId);
        Task<List<OrderViewModel>> GetAllOrders(string status);
        Task UpdateDeliveryStatus(int OrderId, string newStatus);
        Task UpdateOrderStatus(int OrderId, string eventType, string RefNo);
        Task UpdateRefNo(int OrderId, string refNo);
        Task<string> GenerateOrderNumber();
        Task<string> GetLatestOrderByUser(string userId);
        Task<int> GetLatestOrderId(string paymentId = "", string userId = "");

    }
}

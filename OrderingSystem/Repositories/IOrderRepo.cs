using OrderingSystem.Models;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Repositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAllOrders(string status);
        Task<Order> GetOrderById(int Id);
        Task AddtoOrder(Order order);
        Task AddOrderItem(IEnumerable<OrderItem> orderItems);
        Task UpdateStatus(Order order);
        Task<List<Order>> OrderHistoryByUser(string userId);
        Task<List<Order>> GetLatestOrder();
    }
}

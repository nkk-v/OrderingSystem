using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(int Id);
        Task AddtoOrder(Order order);
        Task AddOrderItem(IEnumerable<OrderItem> orderItems);
        Task UpdateOrderStatus(Order order);
    }
}

using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _dbContext;

        public OrderRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrderItem(IEnumerable<OrderItem> orderItems)
        {
            _dbContext.tblOrderItems.AddRange(orderItems);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddtoOrder(Order order)
        {
            _dbContext.tblOrders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Order>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderById(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}

using OrderingSystem.Models;

namespace OrderingSystem.Services
{
    public interface IPaymentService
    {
        Task SavePayment(PayMongoWebhookEvent payMongoWebhook, int orderId);
    }
}

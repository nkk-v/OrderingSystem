using OrderingSystem.Models;
using OrderingSystem.Repositories;

namespace OrderingSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;

        public PaymentService(IPaymentRepo paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task SavePayment(PayMongoWebhookEvent payMongoWebhook, int orderId)
        {
            
            var payment = new Payment
            {
                OrderId = orderId,
                PaymentStatus = payMongoWebhook.Data.Attributes.Data.Attributes.payments[0].Attributes.status,
                PaymentMethod = payMongoWebhook.Data.Attributes.Data.Attributes.payments[0].Attributes.source.Type,
                Amount = payMongoWebhook.Data.Attributes.Data.Attributes.payments[0].Attributes.Amount / 100,
                PaidAt = payMongoWebhook.Data.Attributes.Data.Attributes.PaidAt,
                //UnixTimeStampToDateTime(payMongoWebhook.Data.Attributes.Data.Attributes.PaidAt),
                DateCreated = DateTime.Now

            };

            await _paymentRepo.AddPayment(payment);
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dtDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
            return dtDateTime;
        }
    }
}

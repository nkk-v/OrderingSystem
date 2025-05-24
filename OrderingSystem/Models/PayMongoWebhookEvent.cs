using Newtonsoft.Json;

namespace OrderingSystem.Models
{
    public class PayMongoWebhookEvent
    {
        public PayMongoData Data { get; set; }
    }

    public class PayMongoData
    {
        public PayMongoAttributes Attributes { get; set; }
    }

    public class PayMongoAttributes
    {
        public string Type { get; set; }
        public PayMongoPaymentData Data { get; set; }
    }

    public class PayMongoPaymentData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public PayMongoPaymentAttributes Attributes { get; set; }

    }

    public class PayMongoPaymentAttributes
    {
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }
        [JsonProperty("paid_at")]
        public long PaidAtUnix { get; set; }
        public List<PayMongoPayments> payments { get; set; }

        [JsonProperty("payment_intent_id")]
        public string payment_intent_id { get; set; }
        [JsonIgnore]
        public DateTime PaidAt => DateTimeOffset.FromUnixTimeSeconds(PaidAtUnix).LocalDateTime;
    }

    public class PayMongoPayments
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public PaymentsAttributes Attributes { get; set; }
    }
    
    public class PaymentsAttributes
    {
        public decimal Amount { get; set; }
        public string status { get; set; }
       
        public Source source { get; set; }
    }

    public class Source
    {
        public string Type { get; set; }
    }

}

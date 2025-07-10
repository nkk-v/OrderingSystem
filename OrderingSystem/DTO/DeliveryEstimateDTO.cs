namespace OrderingSystem.DTO
{
    public class DeliveryEstimateDTO
    {
        public double DistanceKm { get; set; }
        public double DurationMinutes { get; set; } // Travel Duration
        public int PreparationMinutes { get; set; } // Total prep time
        public double TotalMinutes => DurationMinutes + PreparationMinutes + 10;
        public DateTime Eta { get; set; } // Final ETA = now + duration + prep
        public double DeliveryFee { get; set; }
        public string? Error { get; set; }
    }
}

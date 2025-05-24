namespace OrderingSystem.DTO
{
    public class AutoCompleteResponseDTO
    {
        public List<FeatureDTO> Features { get; set; } = new();
    }

    public class FeatureDTO
    {
        public GeometryDTO Geometry { get; set; }
        public PropertiesDTO Properties { get; set; }
    }

    public class GeometryDTO
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class PropertiesDTO
    {
        public string Label { get; set; }
    }
}

namespace DynamicConfiguration.Shared.ConfigReader.Models
{
    public class ConfigurationItem
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

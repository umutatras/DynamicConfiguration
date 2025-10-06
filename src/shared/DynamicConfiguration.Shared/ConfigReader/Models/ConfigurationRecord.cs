namespace DynamicConfiguration.Shared.ConfigReader.Models
{
    public class ConfigurationRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!; // string, int, bool, double
        public string Value { get; set; } = null!;
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; } = null!;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

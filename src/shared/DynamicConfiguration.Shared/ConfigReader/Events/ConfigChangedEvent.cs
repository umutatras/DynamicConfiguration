namespace DynamicConfiguration.Shared.ConfigReader.Events
{
    public class ConfigChangedEvent
    {
        public string ApplicationName { get; set; } = null!;
        public string ChangeType { get; set; } = null!; // Added, Updated, Deleted
        public string Name { get; set; } = null!;
        public string? Type { get; set; }
        public string? Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

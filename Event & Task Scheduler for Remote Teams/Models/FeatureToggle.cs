namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class FeatureToggle
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FeatureKey { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace Kaboom.Models.Features
{
    public class PreferenceToggle
    {
        public int Id { get; set; }
        public string? FeatureKey { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

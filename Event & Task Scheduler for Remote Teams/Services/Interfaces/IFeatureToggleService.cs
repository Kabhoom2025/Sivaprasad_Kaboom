using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface IFeatureToggleService
    {
        Task<IEnumerable<FeatureToggle>> GetAllTogglesAsync();
        Task<FeatureToggle> GetToggleAsync(string featureKey);
        Task<FeatureToggle> UpdateToggleAsync(string featureKey, bool isEnabled);
    }
}

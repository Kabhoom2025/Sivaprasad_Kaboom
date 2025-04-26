using System;
using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class FeatureToggleService : IFeatureToggleService
    {
        private readonly ApplicationDbContext _context;

        public FeatureToggleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FeatureToggle>> GetAllTogglesAsync()
        {
            return await _context.FeatureToggles.ToListAsync();
        }

        public async Task<FeatureToggle> GetToggleAsync(string featureKey)
        {
            return await _context.FeatureToggles.FirstOrDefaultAsync(f => f.FeatureKey == featureKey);
        }

        public async Task<FeatureToggle> UpdateToggleAsync(string featureKey, bool isEnabled)
        {
            var toggle = await _context.FeatureToggles.FirstOrDefaultAsync(f => f.FeatureKey == featureKey);
            if (toggle == null)
            {
                toggle = new FeatureToggle
                {
                    FeatureKey = featureKey,
                    IsEnabled = isEnabled
                };
                _context.FeatureToggles.Add(toggle);
            }
            else
            {
                toggle.IsEnabled = isEnabled;
                toggle.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return toggle;
        }
    }
}

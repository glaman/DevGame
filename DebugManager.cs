using System.Collections.Generic;
namespace TurnBasedRPG
{
    public class DebugManager
    {
        public bool DebugModeEnabled { get; set; } = false;
        public bool ShowStatsOverlay { get; set; } = false;

        // Live tweakable values (example)
        public float GlobalDamageMultiplier { get; set; } = 1.0f;
        public float GlobalSpeedMultiplier { get; set; } = 1.0f;

        // You can expand this to hold references to current entities for live editing
        public List<Entity> CurrentlyTrackedEntities { get; set; } = new List<Entity>();

        public void ToggleDebugMode()
        {
            DebugModeEnabled = !DebugModeEnabled;
        }

        public void ToggleStatsOverlay()
        {
            ShowStatsOverlay = !ShowStatsOverlay;
        }
    }
}

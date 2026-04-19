// GameSaveData.cs
using System;
using System.Collections.Generic;

namespace TurnBasedRPG
{
    public class GameSaveData
    {
        public int SaveVersion { get; set; } = 1;

        // Core Player Data
        public PlayerHero Player { get; set; }

        // All unlocked helpers
        public List<HelperHero> UnlockedHelpers { get; set; } = new List<HelperHero>();

        // Current active party (You is always first)
        public List<string> ActivePartyHelperIds { get; set; } = new List<string>();

        // Inventory
        public Inventory PlayerInventory { get; set; } = new Inventory();

        // World / Map state
        public WorldProgress MapProgress { get; set; } = new WorldProgress();

        // Metadata
        public DateTime LastSaved { get; set; } = DateTime.Now;
        public TimeSpan TotalPlayTime { get; set; } = TimeSpan.Zero;

        public GameSaveData()
        {
        }
    }
}

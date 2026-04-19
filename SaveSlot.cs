// SaveSlot.cs
using System;

namespace TurnBasedRPG
{
    public class SaveSlot
    {
        public int SlotIndex { get; set; }                  // 1 to 8
        public string SlotName { get; set; }                // Automatically set to PlayerHero.Name
        public DateTime LastPlayed { get; set; }
        public int PlayerLevel { get; set; }
        public string FurthestPointName { get; set; }
        public bool IsEmpty { get; set; }

        public SaveSlot(int slotIndex)
        {
            SlotIndex = slotIndex;
            IsEmpty = true;
            LastPlayed = DateTime.Now;
        }
    }
}


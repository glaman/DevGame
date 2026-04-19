// EquipmentItem.cs
namespace TurnBasedRPG
{
    public class EquipmentItem
    {
        public string Name { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;   // e.g. "Weapons/Icons/handgun_9mm_icon"
        public string Description { get; set; } = string.Empty;

        // Future stats / effects can go here
        public int MightBonus { get; set; } = 0;
        public int FinesseBonus { get; set; } = 0;
        // etc.

        public EquipmentItem(string name, string iconPath, string description = "")
        {
            Name = name;
            IconPath = iconPath;
            Description = description;
        }
    }
}

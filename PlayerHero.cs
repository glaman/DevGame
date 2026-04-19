// PlayerHero.cs
using System.Text.Json.Serialization;

namespace TurnBasedRPG
{
    public class PlayerHero : Hero
    {
        // Parameterless constructor for JSON
        public PlayerHero() : base() { }

        // Normal constructor
        public PlayerHero(string name, string heroClass) : base(name, heroClass)
        {
        }

        // Player-specific properties
        public EquipmentItem?[] EquippedItems { get; set; } = new EquipmentItem?[4];

        public void EquipItem(int slot, EquipmentItem item)
        {
            if (slot >= 0 && slot < EquippedItems.Length)
            {
                EquippedItems[slot] = item;
            }
        }
    }
}

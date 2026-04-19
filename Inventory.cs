// Inventory.cs
using System.Collections.Generic;

namespace TurnBasedRPG
{
    public class Inventory
    {
        public List<EquipmentItem> Equipment { get; set; } = new List<EquipmentItem>();
        public List<ConsumableItem> Consumables { get; set; } = new List<ConsumableItem>();

        // Helper methods
        public void AddEquipment(EquipmentItem item)
        {
            Equipment.Add(item);
        }

        public void AddConsumable(ConsumableItem item, int quantity = 1)
        {
            // Simple version - we'll expand this later for stacking
            for (int i = 0; i < quantity; i++)
                Consumables.Add(item);
        }
    }
}

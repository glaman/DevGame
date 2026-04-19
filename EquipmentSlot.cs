// EquipmentSlot.cs
namespace TurnBasedRPG
{
    public class EquipmentSlot
    {
        public EquipmentItem EquippedItem { get; set; }
        public EquipmentSlotType SlotType { get; set; }

        public EquipmentSlot(EquipmentSlotType slotType = EquipmentSlotType.None)
        {
            SlotType = slotType;
        }

        public bool IsEmpty => EquippedItem == null;
    }
}

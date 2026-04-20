// PlayerHero.cs
namespace TurnBasedRPG
{
    public class PlayerHero : Hero
    {
        // Parameterless constructor for JSON
        public PlayerHero()
            : base() { }

        // Normal constructor
        public PlayerHero(string name, string heroClass)
            : base(name, heroClass)
        {
            // Default starting values
            Level = 1;
            CurrentExp = 0;
            MaxExp = 100; // EXP needed to reach level 2

            CurrentHP = MaxHP = 100;

            // Starting stats (you can adjust these later)
            Might = 10;
            Finesse = 10;
            Wit = 10;
            Vigor = 10;
            Speed = 10;
        }

        // Player-specific properties
        public EquipmentItem?[] EquippedItems { get; set; } = new EquipmentItem?[4];

        // === NEW: EXP and Level System ===
        public int CurrentExp { get; set; } = 0;
        public int MaxExp { get; set; } = 100;

        // Optional: Simple method to add experience (useful for testing and battles later)
        public void AddExperience(int amount)
        {
            if (amount <= 0)
                return;

            CurrentExp += amount;

            while (CurrentExp >= MaxExp && Level < 99) // safety cap
            {
                CurrentExp -= MaxExp;
                Level++;

                // Increase requirements and stats on level up
                MaxExp = (int)(MaxExp * 1.25f); // 25% more EXP per level
                MaxHP += 25;
                CurrentHP = MaxHP;

                // Small stat increases (you can tune these later)
                Might += 2;
                Finesse += 1;
                Wit += 1;
                Vigor += 2;
                Speed += 1;
            }
        }

        public void EquipItem(int slot, EquipmentItem item)
        {
            if (slot >= 0 && slot < EquippedItems.Length)
            {
                EquippedItems[slot] = item;
            }
        }
    }
}

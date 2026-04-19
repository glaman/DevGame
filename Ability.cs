// Ability.cs
namespace TurnBasedRPG
{
    public class Ability
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }           // Cooldown in rounds (0 = no cooldown)
        public int CurrentCooldown { get; set; }    // Remaining rounds until usable

        public AbilityType Type { get; set; }       // Attack, Heal, Buff, etc.

        public Ability(string name, string description, int cooldown, AbilityType type)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            CurrentCooldown = 0;
            Type = type;
        }

        // Helper to check if ability is ready
        public bool IsReady => CurrentCooldown <= 0;

        // Called at the end of each full round
        public void TickCooldown()
        {
            if (CurrentCooldown > 0)
                CurrentCooldown--;
        }
    }
}

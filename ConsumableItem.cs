namespace TurnBasedRPG
{
    public class ConsumableItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } = 1;

        // Placeholder until we define the effect system
        public string EffectDescription { get; set; }

        public ConsumableItem(string name, string description, string effectDescription)
        {
            Name = name;
            Description = description;
            EffectDescription = effectDescription;
        }
    }
}

// Hero.cs - Clean version (no property hiding)
using System.Text.Json.Serialization;

namespace TurnBasedRPG
{
    public class Hero : Entity
    {
        // Parameterless constructor for JSON
        public Hero() : base("", "") { }

        // Normal constructor
        public Hero(string name, string heroClass) : base(name, heroClass) { }

        // Add any Hero-specific properties/methods here later
    }
}

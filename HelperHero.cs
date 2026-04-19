// HelperHero.cs
using System.Text.Json.Serialization;

namespace TurnBasedRPG
{
    public class HelperHero : Hero
    {
        // Parameterless constructor for JSON
        public HelperHero() : base() { }

        // Normal constructor
        public HelperHero(string name, string heroClass) : base(name, heroClass)
        {
        }
    }
}

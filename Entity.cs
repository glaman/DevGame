// Entity.cs - Base class for all characters
using System;

namespace TurnBasedRPG
{
    public abstract class Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;

        // Primary Stats
        public int Might { get; set; } = 10;
        public int Finesse { get; set; } = 10;
        public int Wit { get; set; } = 10;
        public int Vigor { get; set; } = 10;

        // Secondary Stats
        public int Speed { get; set; } = 10;
        public int Strike { get; set; } = 10;
        public int Arcane { get; set; } = 10;
        public int Hardiness { get; set; } = 10;
        public int Resolve { get; set; } = 10;

        // Combat Values
        public int Level { get; set; } = 1;
        public int CurrentHP { get; set; }
        public int MaxHP { get; set; }

        protected Entity() {}
        protected Entity(string name, string heroClass)
        {
            Name = name;
            Class = heroClass;
            MaxHP = 100 + (Vigor * 5);
            CurrentHP = MaxHP;
        }
    }
}

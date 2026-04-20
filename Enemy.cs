// Enemy.cs
using System;

namespace TurnBasedRPG
{
    public class Enemy : Entity
    {
        public Enemy(string name)
            : base(name, "Enemy")
        {
            // Default enemy stats - easy to tweak later
            Might = 8;
            Finesse = 6;
            Wit = 5;
            Vigor = 10;
            Speed = 7;
        }

        // You can add enemy-specific fields here later (e.g. threat level, loot table, etc.)
    }
}

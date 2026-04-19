// BattleManager.cs - Updated to work with current Hero class
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TurnBasedRPG
{
    public class BattleManager
    {
        private List<Hero> _playerParty;
        private List<Hero> _enemies;

        public BattleManager(List<Hero> playerParty, List<Hero> enemies)
        {
            _playerParty = playerParty ?? new List<Hero>();
            _enemies = enemies ?? new List<Hero>();
        }

        public void Update(GameTime gameTime)
        {
            // Basic battle logic placeholder
            // We'll expand this later with turn order, cooldowns, etc.
            Console.WriteLine("Battle Update - Player party size: " + _playerParty.Count);
        }

        // Simple method to get all units for turn order (for future expansion)
        public List<Hero> GetAllUnits()
        {
            var allUnits = new List<Hero>();
            allUnits.AddRange(_playerParty);
            allUnits.AddRange(_enemies);
            return allUnits;
        }

        public bool IsBattleOver()
        {
            bool playerAlive = _playerParty.Any(h => h.CurrentHP > 0);
            bool enemyAlive = _enemies.Any(h => h.CurrentHP > 0);
            return !playerAlive || !enemyAlive;
        }
    }
}

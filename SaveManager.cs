// SaveManager.cs - Fixed to use int slot numbers
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace TurnBasedRPG
{
    public class SaveManager
    {
        private readonly string _saveDirectory;

        public SaveManager()
        {
            _saveDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves");
            Directory.CreateDirectory(_saveDirectory);
        }

        // Save to a numbered slot (1-8)
        public bool SaveGame(Game1 game, int slotNumber)
        {
            if (slotNumber < 1 || slotNumber > 8) slotNumber = 1;
            string saveName = $"PlayerSave{slotNumber}";

            try
            {
                if (game.Player == null)
                {
                    Console.WriteLine("ERROR: No player to save.");
                    return false;
                }

                var saveData = new GameSaveData
                {
                    Player = game.Player,
                    ActivePartyHelperIds = new List<string>(),
                    LastSaved = DateTime.Now
                };

                foreach (var hero in game.ActiveParty)
                {
                    if (hero is HelperHero helper)
                        saveData.ActivePartyHelperIds.Add(helper.Name);
                }

                string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                });

                string filePath = Path.Combine(_saveDirectory, $"{saveName}.json");
                File.WriteAllText(filePath, json);

                Console.WriteLine($"✅ Saved to Slot {slotNumber}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save failed: {ex.Message}");
                return false;
            }
        }

        // Load from a numbered slot (1-8)
        public bool LoadGame(Game1 game, int slotNumber)
        {
            if (slotNumber < 1 || slotNumber > 8) slotNumber = 1;
            string saveName = $"PlayerSave{slotNumber}";

            try
            {
                string filePath = Path.Combine(_saveDirectory, $"{saveName}.json");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Slot {slotNumber} is empty.");
                    return false;
                }

                string json = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };

                var saveData = JsonSerializer.Deserialize<GameSaveData>(json, options);

                if (saveData?.Player == null)
                {
                    Console.WriteLine("Invalid save data.");
                    return false;
                }

                game.Player = saveData.Player;
                game.ActiveParty.Clear();
                game.ActiveParty.Add(game.Player);

                if (saveData.ActivePartyHelperIds != null)
                {
                    foreach (var name in saveData.ActivePartyHelperIds)
                    {
                        var match = game.ActiveParty.Find(h => h is HelperHero && h.Name == name);
                        if (match != null)
                            game.ActiveParty.Add(match);
                    }
                }

                Console.WriteLine($"✅ Loaded Slot {slotNumber} - {game.Player.Name}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load failed: {ex.Message}");
                return false;
            }
        }

        public string GetSaveDirectory() => _saveDirectory;
    }
}

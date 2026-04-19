// MainHubScreen.cs - Cleaned version (no unused variable warnings)
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class MainHubScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        // Icon caches
        private Dictionary<string, Texture2D?> _weaponIconCache =
            new Dictionary<string, Texture2D?>();
        private Dictionary<string, Texture2D?> _heroIconCache =
            new Dictionary<string, Texture2D?>();
        private Texture2D? _playerAvatar;

        private Rectangle _topMenuRect;
        private Rectangle _equipmentBtnRect;
        private Rectangle _objectivesBtnRect;
        private Rectangle _heroesBtnRect;

        private Rectangle _loadoutRect;
        private Rectangle _mapRect;
        private Rectangle _teamRect;

        private Rectangle[] _loadoutSlotRects = new Rectangle[4];
        private Rectangle[] _teamHeroBoxes = new Rectangle[2];
        private Rectangle[] _teamAvatarRects = new Rectangle[2];
        private Rectangle _playerAvatarRect;

        private Rectangle[] _miniMapSelectionRects = new Rectangle[5];

        public MainHubScreen(Game1 game)
            : base(game) { }

        public override void LoadContent()
        {
            _whitePixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new Color[] { Color.White });

            try
            {
                _font = Game.Content.Load<SpriteFont>("DefaultFont");
            }
            catch
            {
                _font = null;
            }

            // Load icons
            LoadWeaponIcons();
            LoadHeroIcons();
            LoadPlayerAvatar();

            // Layout rectangles
            _topMenuRect = new Rectangle(0, 0, 1920, 110);

            _equipmentBtnRect = new Rectangle(120, 850, 260, 140);
            _objectivesBtnRect = new Rectangle(420, 850, 800, 140);
            _heroesBtnRect = new Rectangle(1260, 850, 260, 140);

            _loadoutRect = new Rectangle(80, 150, 340, 620);
            _mapRect = new Rectangle(460, 150, 400, 620);
            _teamRect = new Rectangle(1480, 150, 360, 620);

            // Player avatar area above loadout
            _playerAvatarRect = new Rectangle(140, 170, 120, 120);

            // Loadout slots (shifted down)
            int slotY = 310;
            for (int i = 0; i < 4; i++)
            {
                _loadoutSlotRects[i] = new Rectangle(100, slotY, 300, 65);
                slotY += 75;
            }

            // Team boxes
            _teamHeroBoxes[0] = new Rectangle(1510, 220, 300, 240);
            _teamHeroBoxes[1] = new Rectangle(1510, 490, 300, 240);

            _teamAvatarRects[0] = new Rectangle(1530, 245, 140, 140);
            _teamAvatarRects[1] = new Rectangle(1530, 515, 140, 140);

            // Map selection list
            int mapY = 220;
            for (int i = 0; i < 5; i++)
            {
                _miniMapSelectionRects[i] = new Rectangle(480, mapY, 360, 55);
                mapY += 70;
            }
        }

        private void LoadWeaponIcons()
        {
            var keys = new[]
            {
                "handgun_9mm_icon",
                "rusty_revolver",
                "compound_bow",
                "energy_blaster",
            };
            foreach (var key in keys)
            {
                string fullPath = Path.Combine(
                    Game.Content.RootDirectory,
                    "Weapons/Icons",
                    key + ".png"
                );
                if (File.Exists(fullPath))
                    _weaponIconCache[key] = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
            }
        }

        private void LoadHeroIcons()
        {
            var classIcons = new[]
            {
                "brawler",
                "sharpshooter",
                "healer",
                "magician",
                "jack_of_all_trades",
                "player_male",
            };
            foreach (var cls in classIcons)
            {
                string fullPath = Path.Combine(
                    Game.Content.RootDirectory,
                    "Characters/Icons",
                    cls + ".png"
                );
                if (File.Exists(fullPath))
                    _heroIconCache[cls] = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
            }
        }

        private void LoadPlayerAvatar()
        {
            string fullPath = Path.Combine(
                Game.Content.RootDirectory,
                "Characters/Icons/player_male.png"
            );
            if (File.Exists(fullPath))
            {
                try
                {
                    _playerAvatar = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
                    Console.WriteLine("MainHub: Loaded player avatar");
                }
                catch
                {
                    _playerAvatar = null;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Game.Window.Title = $"DevGame - Main Hub - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        protected override void OnMouseClick(Point mousePosition)
        {
            if (_equipmentBtnRect.Contains(mousePosition))
            {
                Game.ChangeState(GameState.Equipment);
                return;
            }
            if (_heroesBtnRect.Contains(mousePosition))
            {
                Game.ChangeState(GameState.HeroGallery);
                return;
            }

            // Loadout slots
            for (int i = 0; i < 4; i++)
            {
                if (_loadoutSlotRects[i].Contains(mousePosition) && Game.EquipmentScreen != null)
                {
                    Game.EquipmentScreen.SetSelectedSlot(i, this);
                    Game.ChangeState(GameState.Equipment);
                    return;
                }
            }

            // Team heroes
            int helperIndex = 0;
            for (int i = 0; i < Game.ActiveParty.Count; i++)
            {
                var hero = Game.ActiveParty[i];
                if (hero is PlayerHero)
                    continue;

                if (helperIndex < 2 && _teamHeroBoxes[helperIndex].Contains(mousePosition))
                {
                    Game.ShowHeroDetail(hero);
                    return;
                }
                helperIndex++;
            }

            // Map selection
            if (Game.MapScreen != null)
            {
                for (
                    int i = 0;
                    i < Game.MapScreen.GetMapCount() && i < _miniMapSelectionRects.Length;
                    i++
                )
                {
                    if (_miniMapSelectionRects[i].Contains(mousePosition))
                    {
                        Game.MapScreen.SetCurrentMap(i);
                        Game.ChangeState(GameState.Map);
                        return;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(18, 16, 32));

            if (_font != null)
            {
                // Top bar
                spriteBatch.Draw(_whitePixel, _topMenuRect, new Color(32, 28, 55));
                spriteBatch.DrawString(_font, "MAIN HUB", new Vector2(80, 35), Color.White);

                // Bottom navigation bar
                spriteBatch.Draw(
                    _whitePixel,
                    new Rectangle(0, 810, 1920, 270),
                    new Color(28, 24, 50)
                );

                spriteBatch.Draw(_whitePixel, _equipmentBtnRect, new Color(70, 50, 95));
                spriteBatch.DrawString(_font, "EQUIPMENT", new Vector2(160, 900), Color.White);

                spriteBatch.Draw(_whitePixel, _objectivesBtnRect, new Color(55, 45, 85));
                spriteBatch.DrawString(_font, "OBJECTIVES", new Vector2(520, 900), Color.White);

                spriteBatch.Draw(_whitePixel, _heroesBtnRect, new Color(70, 50, 95));
                spriteBatch.DrawString(_font, "HEROES", new Vector2(1300, 900), Color.White);

                // Current Loadout Section
                spriteBatch.Draw(_whitePixel, _loadoutRect, new Color(38, 33, 68));

                // Player Avatar
                spriteBatch.Draw(_whitePixel, _playerAvatarRect, new Color(50, 45, 80));
                if (_playerAvatar != null)
                {
                    spriteBatch.Draw(
                        _playerAvatar,
                        new Rectangle(_playerAvatarRect.X + 10, _playerAvatarRect.Y + 10, 100, 100),
                        Color.White
                    );
                }
                else
                {
                    spriteBatch.DrawString(
                        _font,
                        "PLAYER",
                        new Vector2(_playerAvatarRect.X + 25, _playerAvatarRect.Y + 45),
                        Color.LightGray
                    );
                }

                spriteBatch.DrawString(_font, "CURRENT LOADOUT", new Vector2(140, 305), Color.Cyan);

                // Loadout slots
                if (Game.Player != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var slotRect = _loadoutSlotRects[i];
                        spriteBatch.Draw(_whitePixel, slotRect, new Color(50, 45, 75));

                        var item = Game.Player.EquippedItems?[i];
                        if (item != null)
                        {
                            string key = item.IconPath.Replace("Weapons/Icons/", "");
                            if (_weaponIconCache.TryGetValue(key, out var icon) && icon != null)
                            {
                                spriteBatch.Draw(
                                    icon,
                                    new Rectangle(slotRect.X + 10, slotRect.Y + 5, 55, 55),
                                    Color.White
                                );
                            }
                            spriteBatch.DrawString(
                                _font,
                                item.Name,
                                new Vector2(slotRect.X + 80, slotRect.Y + 18),
                                Color.LightGreen
                            );
                        }
                        else
                        {
                            spriteBatch.DrawString(
                                _font,
                                $"Slot {i + 1} - Empty",
                                new Vector2(slotRect.X + 20, slotRect.Y + 20),
                                Color.LightGray
                            );
                        }
                    }
                }

                // Map area
                spriteBatch.Draw(_whitePixel, _mapRect, new Color(25, 38, 52));
                spriteBatch.DrawString(_font, "AVAILABLE MAPS", new Vector2(520, 180), Color.Cyan);

                if (Game.MapScreen != null)
                {
                    for (
                        int i = 0;
                        i < Game.MapScreen.GetMapCount() && i < _miniMapSelectionRects.Length;
                        i++
                    )
                    {
                        string mapName = Game.MapScreen.GetMapName(i);
                        bool isCurrent = i == Game.MapScreen.GetCurrentMapIndex();
                        var rect = _miniMapSelectionRects[i];
                        spriteBatch.Draw(
                            _whitePixel,
                            rect,
                            isCurrent ? new Color(80, 60, 120) : new Color(45, 40, 75)
                        );
                        spriteBatch.DrawString(
                            _font,
                            mapName,
                            new Vector2(rect.X + 30, rect.Y + 15),
                            isCurrent ? Color.Yellow : Color.White
                        );
                    }
                }

                // Current Team
                spriteBatch.Draw(_whitePixel, _teamRect, new Color(38, 33, 68));
                spriteBatch.DrawString(
                    _font,
                    "CURRENT TEAM (2/2)",
                    new Vector2(1510, 180),
                    Color.Cyan
                );

                int helperIndex = 0;
                for (int i = 0; i < Game.ActiveParty.Count; i++)
                {
                    var hero = Game.ActiveParty[i];
                    if (hero is PlayerHero)
                        continue;
                    if (helperIndex >= 2)
                        break;

                    var boxRect = _teamHeroBoxes[helperIndex];
                    var avatarRect = _teamAvatarRects[helperIndex];

                    spriteBatch.Draw(_whitePixel, boxRect, new Color(48, 42, 75));

                    string classKey = hero.Class.ToLower().Replace(" ", "_").Replace("-", "_");
                    if (
                        _heroIconCache.TryGetValue(classKey, out var avatarIcon)
                        && avatarIcon != null
                    )
                    {
                        spriteBatch.Draw(avatarIcon, avatarRect, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_whitePixel, avatarRect, new Color(60, 55, 85));
                        spriteBatch.DrawString(
                            _font,
                            "AVATAR",
                            new Vector2(avatarRect.X + 35, avatarRect.Y + 55),
                            Color.LightGray
                        );
                    }

                    spriteBatch.DrawString(
                        _font,
                        hero.Name,
                        new Vector2(boxRect.X + 25, boxRect.Y + 195),
                        Color.White
                    );
                    spriteBatch.DrawString(
                        _font,
                        $"({hero.Class})",
                        new Vector2(boxRect.X + 25, boxRect.Y + 220),
                        Color.Cyan
                    );

                    helperIndex++;
                }

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}

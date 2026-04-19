// PlayerProfileScreen.cs - Back button now goes directly to Dashboard
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace TurnBasedRPG
{
    public class PlayerProfileScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        private Rectangle _topMenuRect;
        private Rectangle _backButtonRect;

        private Rectangle _avatarRect;
        private Rectangle _nameRect;
        private Rectangle _statsRect;
        private Rectangle[] _equipmentSlots = new Rectangle[4];

        private Dictionary<string, Texture2D?> _equippedIconCache = new Dictionary<string, Texture2D?>();
        private Texture2D? _playerAvatar;

        public PlayerProfileScreen(Game1 game) : base(game)
        {
        }

        public override void LoadContent()
        {
            _whitePixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new Color[] { Color.White });

            try
            {
                _font = Game.Content.Load<SpriteFont>("DefaultFont");
            }
            catch { _font = null; }

            _topMenuRect = new Rectangle(0, 0, 1920, 110);
            _backButtonRect = new Rectangle(80, 25, 260, 60);

            _avatarRect = new Rectangle(180, 160, 580, 580);
            _nameRect = new Rectangle(860, 180, 900, 100);
            _statsRect = new Rectangle(860, 300, 900, 420);

            int slotX = 220;
            for (int i = 0; i < 4; i++)
            {
                _equipmentSlots[i] = new Rectangle(slotX, 820, 140, 140);
                slotX += 180;
            }

            LoadPlayerAvatar();
            LoadEquippedIcons();
        }

        private void LoadPlayerAvatar()
        {
            string fullPath = Path.Combine(Game.Content.RootDirectory, "Characters/Icons/player_male.png");
            if (File.Exists(fullPath))
            {
                try
                {
                    _playerAvatar = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
                }
                catch { _playerAvatar = null; }
            }
        }

        private void LoadEquippedIcons()
        {
            var commonKeys = new[] { "handgun_9mm_icon", "rusty_revolver", "compound_bow", "energy_blaster" };
            foreach (var key in commonKeys)
            {
                string fullPath = Path.Combine(Game.Content.RootDirectory, "Weapons/Icons", key + ".png");
                if (File.Exists(fullPath))
                {
                    try
                    {
                        var icon = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
                        _equippedIconCache[key] = icon;
                    }
                    catch { }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Game.Window.Title = $"DevGame - Player Profile - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        protected override void OnMouseClick(Point mousePosition)
        {
            // Back button - Always go to Dashboard (Main Hub)
            if (_backButtonRect.Contains(mousePosition))
            {
                Game.ChangeState(GameState.MainHub);
                return;
            }

            if (_avatarRect.Contains(mousePosition))
            {
                Console.WriteLine("Avatar clicked - TODO: customization");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (_equipmentSlots[i].Contains(mousePosition))
                {
                    if (Game.EquipmentScreen != null)
                    {
                        Game.EquipmentScreen.SetSelectedSlot(i, this);
                        Game.ChangeState(GameState.Equipment);
                    }
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(25, 20, 45));

            if (_font != null)
            {
                spriteBatch.Draw(_whitePixel, _topMenuRect, new Color(35, 30, 55));
                spriteBatch.DrawString(_font, "PLAYER PROFILE", new Vector2(420, 35), Color.White);

                spriteBatch.Draw(_whitePixel, _backButtonRect, new Color(100, 30, 30));
                spriteBatch.DrawString(_font, "BACK", new Vector2(160, 42), Color.White);

                // Avatar
                spriteBatch.Draw(_whitePixel, _avatarRect, new Color(40, 35, 65));
                if (_playerAvatar != null)
                {
                    spriteBatch.Draw(_playerAvatar, new Rectangle(_avatarRect.X + 40, _avatarRect.Y + 40, 500, 500), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(_font, "AVATAR", new Vector2(420, 380), Color.LightGray);
                }

                // Name
                spriteBatch.Draw(_whitePixel, _nameRect, new Color(50, 45, 80));
                spriteBatch.DrawString(_font, "NAME:", new Vector2(920, 205), Color.Cyan);
                if (Game.Player != null)
                    spriteBatch.DrawString(_font, Game.Player.Name, new Vector2(1120, 205), Color.White);

                // Stats
                spriteBatch.Draw(_whitePixel, _statsRect, new Color(35, 30, 60));
                spriteBatch.DrawString(_font, "STATS", new Vector2(920, 325), Color.Cyan);

                if (Game.Player != null)
                {
                    int y = 380;
                    spriteBatch.DrawString(_font, $"Might:   {Game.Player.Might}", new Vector2(920, y), Color.White); y += 48;
                    spriteBatch.DrawString(_font, $"Finesse: {Game.Player.Finesse}", new Vector2(920, y), Color.White); y += 48;
                    spriteBatch.DrawString(_font, $"Wit:     {Game.Player.Wit}", new Vector2(920, y), Color.White); y += 48;
                    spriteBatch.DrawString(_font, $"Vigor:   {Game.Player.Vigor}", new Vector2(920, y), Color.White); y += 48;
                    spriteBatch.DrawString(_font, $"Speed:   {Game.Player.Speed}", new Vector2(920, y), Color.White);
                }

                // Equipment
                spriteBatch.Draw(_whitePixel, new Rectangle(180, 780, 1620, 240), new Color(45, 40, 75));
                spriteBatch.DrawString(_font, "EQUIPPED ITEMS (Click any slot to change)", new Vector2(220, 800), Color.Cyan);

                for (int i = 0; i < 4; i++)
                {
                    var slotRect = _equipmentSlots[i];
                    spriteBatch.Draw(_whitePixel, slotRect, new Color(60, 55, 90));

                    var item = Game.Player?.EquippedItems?[i];
                    if (item != null)
                    {
                        string key = item.IconPath.Replace("Weapons/Icons/", "");
                        if (_equippedIconCache.TryGetValue(key, out var icon) && icon != null)
                        {
                            spriteBatch.Draw(icon, new Rectangle(slotRect.X + 10, slotRect.Y + 10, 120, 120), Color.White);
                        }
                        spriteBatch.DrawString(_font, item.Name, new Vector2(slotRect.X + 10, slotRect.Y + 145), Color.LightGreen);
                    }
                    else
                    {
                        spriteBatch.DrawString(_font, $"Slot {i + 1}", new Vector2(slotRect.X + 35, slotRect.Y + 55), Color.LightGray);
                    }
                }

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}

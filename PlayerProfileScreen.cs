// PlayerProfileScreen.cs - Fixed ADD/CHANGE button clicks
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class PlayerProfileScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        private Rectangle _topMenuRect;
        private Rectangle _backButtonRect;

        private Rectangle _avatarRect;
        private Rectangle _headshotRect;
        private Rectangle _nameRect;
        private Rectangle _statsRect;

        private Rectangle[] _equipmentBoxes = new Rectangle[4];
        private Rectangle[] _actionButtonRects = new Rectangle[4]; // ADD or CHANGE
        private Rectangle[] _removeButtonRects = new Rectangle[4]; // REMOVE

        private Dictionary<string, Texture2D?> _equippedIconCache =
            new Dictionary<string, Texture2D?>();
        private Texture2D? _playerFullBody;
        private Texture2D? _playerHeadshot;

        private EquipmentItem? _selectedItemForInfo;
        private float _infoDisplayTime = 0f;

        public PlayerProfileScreen(Game1 game)
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

            _topMenuRect = new Rectangle(0, 0, 1920, 110);
            _backButtonRect = new Rectangle(80, 25, 260, 60);

            _avatarRect = new Rectangle(180, 160, 580, 580);
            _nameRect = new Rectangle(860, 180, 900, 120);
            _headshotRect = new Rectangle(880, 195, 90, 90);
            _statsRect = new Rectangle(860, 330, 900, 420);

            // Equipment boxes
            for (int i = 0; i < 4; i++)
            {
                _equipmentBoxes[i] = new Rectangle(180 + i * 390, 780, 340, 180);

                // Action button (ADD / CHANGE)
                _actionButtonRects[i] = new Rectangle(460 + i * 390, 810, 110, 45);

                // Remove button
                _removeButtonRects[i] = new Rectangle(460 + i * 390, 865, 110, 45);
            }

            LoadPlayerFullBody();
            LoadPlayerHeadshot();
            LoadEquippedIcons();
        }

        private void LoadPlayerFullBody()
        {
            string path = Path.Combine(
                Game.Content.RootDirectory,
                "Characters/FullBody/player_male_fullbody_action.png"
            );
            if (File.Exists(path))
            {
                try
                {
                    _playerFullBody = Texture2D.FromFile(Game.GraphicsDevice, path);
                }
                catch
                {
                    _playerFullBody = null;
                }
            }
        }

        private void LoadPlayerHeadshot()
        {
            string path = Path.Combine(
                Game.Content.RootDirectory,
                "Characters/Icons/player_male.png"
            );
            if (File.Exists(path))
            {
                try
                {
                    _playerHeadshot = Texture2D.FromFile(Game.GraphicsDevice, path);
                }
                catch
                {
                    _playerHeadshot = null;
                }
            }
        }

        private void LoadEquippedIcons()
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
                string path = Path.Combine(
                    Game.Content.RootDirectory,
                    "Weapons/Icons",
                    key + ".png"
                );
                if (File.Exists(path))
                {
                    try
                    {
                        _equippedIconCache[key] = Texture2D.FromFile(Game.GraphicsDevice, path);
                    }
                    catch { }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            if (_infoDisplayTime > 0)
                _infoDisplayTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Game.Window.Title = $"DevGame - Player Profile - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        public override void OnMouseClick(Point mousePosition)
        {
            if (_backButtonRect.Contains(mousePosition))
            {
                Game.ChangeState(GameState.MainHub);
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                var box = _equipmentBoxes[i];
                var actionBtn = _actionButtonRects[i];
                var removeBtn = _removeButtonRects[i];
                var item = Game.Player?.EquippedItems?[i];

                // Click main box area → show item info
                if (box.Contains(mousePosition))
                {
                    if (item != null)
                    {
                        _selectedItemForInfo = item;
                        _infoDisplayTime = 4.0f;
                    }
                    return;
                }

                // Click ADD / CHANGE button
                if (actionBtn.Contains(mousePosition) && Game.EquipmentScreen != null)
                {
                    Game.EquipmentScreen.SetSelectedSlot(i, this);
                    Game.ChangeState(GameState.Equipment);
                    return;
                }

                // Click REMOVE button
                if (item != null && removeBtn.Contains(mousePosition))
                {
                    if (Game.Player != null)
                    {
                        Game.Player.EquippedItems[i] = null;
                        Console.WriteLine($"Removed item from slot {i + 1}");
                    }
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(25, 20, 45));

            if (_font == null)
                return;

            // Top menu
            spriteBatch.Draw(_whitePixel, _topMenuRect, new Color(35, 30, 55));
            spriteBatch.DrawString(_font, "PLAYER PROFILE", new Vector2(420, 35), Color.White);
            spriteBatch.Draw(_whitePixel, _backButtonRect, new Color(100, 30, 30));
            spriteBatch.DrawString(_font, "BACK", new Vector2(160, 42), Color.White);

            // Full body
            spriteBatch.Draw(_whitePixel, _avatarRect, new Color(40, 35, 65));
            if (_playerFullBody != null)
            {
                int w = (int)(_avatarRect.Width * 0.82f);
                int h = (int)(_avatarRect.Height * 0.78f);
                int x = _avatarRect.X + (_avatarRect.Width - w) / 2;
                int y = _avatarRect.Y + (_avatarRect.Height - h) / 2;
                spriteBatch.Draw(_playerFullBody, new Rectangle(x, y, w, h), Color.White);
            }

            // Name + headshot
            spriteBatch.Draw(_whitePixel, _nameRect, new Color(50, 45, 80));
            if (_playerHeadshot != null)
                spriteBatch.Draw(_playerHeadshot, _headshotRect, Color.White);

            if (Game.Player != null)
            {
                string nameLevel = $"{Game.Player.Name} - Level {Game.Player.Level}";
                spriteBatch.DrawString(_font, nameLevel, new Vector2(980, 205), Color.White);
            }

            // EXP and HEALTH
            if (Game.Player != null)
            {
                spriteBatch.DrawString(
                    _font,
                    $"EXP: {Game.Player.CurrentExp} / {Game.Player.MaxExp}",
                    new Vector2(980, 265),
                    Color.LightGreen
                );

                spriteBatch.DrawString(
                    _font,
                    $"HEALTH: {Game.Player.CurrentHP} / {Game.Player.MaxHP}",
                    new Vector2(1380, 265),
                    Color.Red
                );
            }

            // Stats
            spriteBatch.Draw(_whitePixel, _statsRect, new Color(35, 30, 60));
            spriteBatch.DrawString(_font, "STATS", new Vector2(920, 355), Color.Cyan);

            if (Game.Player != null)
            {
                int y = 410;
                spriteBatch.DrawString(
                    _font,
                    $"Might: {Game.Player.Might}",
                    new Vector2(920, y),
                    Color.White
                );
                y += 48;
                spriteBatch.DrawString(
                    _font,
                    $"Finesse: {Game.Player.Finesse}",
                    new Vector2(920, y),
                    Color.White
                );
                y += 48;
                spriteBatch.DrawString(
                    _font,
                    $"Wit: {Game.Player.Wit}",
                    new Vector2(920, y),
                    Color.White
                );
                y += 48;
                spriteBatch.DrawString(
                    _font,
                    $"Vigor: {Game.Player.Vigor}",
                    new Vector2(920, y),
                    Color.White
                );
                y += 48;
                spriteBatch.DrawString(
                    _font,
                    $"Speed: {Game.Player.Speed}",
                    new Vector2(920, y),
                    Color.White
                );
            }

            // Equipment Boxes with buttons inside
            if (Game.Player != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    var box = _equipmentBoxes[i];
                    var item = Game.Player.EquippedItems?[i];

                    spriteBatch.Draw(_whitePixel, box, new Color(45, 40, 75));

                    if (item != null)
                    {
                        // Icon
                        string key = item.IconPath.Replace("Weapons/Icons/", "");
                        if (_equippedIconCache.TryGetValue(key, out var icon) && icon != null)
                        {
                            spriteBatch.Draw(
                                icon,
                                new Rectangle(box.X + 20, box.Y + 25, 80, 80),
                                Color.White
                            );
                        }

                        spriteBatch.DrawString(
                            _font,
                            item.Name,
                            new Vector2(box.X + 120, box.Y + 35),
                            Color.LightGreen
                        );

                        // CHANGE button
                        spriteBatch.Draw(
                            _whitePixel,
                            _actionButtonRects[i],
                            new Color(70, 100, 70)
                        );
                        spriteBatch.DrawString(
                            _font,
                            "CHANGE",
                            new Vector2(_actionButtonRects[i].X + 12, _actionButtonRects[i].Y + 10),
                            Color.White
                        );

                        // REMOVE button
                        spriteBatch.Draw(
                            _whitePixel,
                            _removeButtonRects[i],
                            new Color(140, 40, 40)
                        );
                        spriteBatch.DrawString(
                            _font,
                            "REMOVE",
                            new Vector2(_removeButtonRects[i].X + 12, _removeButtonRects[i].Y + 10),
                            Color.White
                        );
                    }
                    else
                    {
                        spriteBatch.DrawString(
                            _font,
                            "Empty Slot",
                            new Vector2(box.X + 120, box.Y + 65),
                            Color.LightGray
                        );

                        // ADD button
                        spriteBatch.Draw(
                            _whitePixel,
                            _actionButtonRects[i],
                            new Color(60, 80, 140)
                        );
                        spriteBatch.DrawString(
                            _font,
                            "ADD",
                            new Vector2(_actionButtonRects[i].X + 30, _actionButtonRects[i].Y + 10),
                            Color.White
                        );
                    }
                }
            }

            // Temporary item info overlay
            if (_selectedItemForInfo != null && _infoDisplayTime > 0)
            {
                spriteBatch.Draw(
                    _whitePixel,
                    new Rectangle(600, 480, 720, 180),
                    new Color(20, 20, 35, 240)
                );
                spriteBatch.DrawString(_font, "ITEM DETAILS", new Vector2(640, 500), Color.Cyan);
                spriteBatch.DrawString(
                    _font,
                    $"Name: {_selectedItemForInfo.Name}",
                    new Vector2(640, 540),
                    Color.White
                );
                spriteBatch.DrawString(
                    _font,
                    $"Description: {_selectedItemForInfo.Description ?? "No description"}",
                    new Vector2(640, 570),
                    Color.LightGray
                );
            }

            DrawDevMenu(spriteBatch, _font);
        }
    }
}

// EquipmentScreen.cs - Proper icon per item
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class EquipmentScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        private int _selectedSlot = -1;
        private Screen? _previousScreen;

        private List<EquipmentItem> _inventoryItems = new List<EquipmentItem>();
        private List<Texture2D?> _loadedIcons = new List<Texture2D?>();

        private Rectangle[] _inventoryRects = new Rectangle[8];
        private Rectangle _backButtonRect;

        public EquipmentScreen(Game1 game)
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

            // === Define all equipment items ===
            _inventoryItems.Clear();
            _loadedIcons.Clear();

            var items = new[]
            {
                new EquipmentItem(
                    "9mm Handgun",
                    "Weapons/Icons/handgun_9mm_icon",
                    "Basic starter pistol"
                ),
                new EquipmentItem(
                    "Rusty Revolver",
                    "Weapons/Icons/rusty_revolver",
                    "Old reliable sidearm"
                ),
                new EquipmentItem(
                    "Compound Bow",
                    "Weapons/Icons/compound_bow",
                    "Silent powerful bow"
                ),
                new EquipmentItem(
                    "Energy Blaster",
                    "Weapons/Icons/energy_blaster",
                    "Futuristic plasma weapon"
                ),
            };

            foreach (var item in items)
            {
                _inventoryItems.Add(item);

                string fullPath = Path.Combine(Game.Content.RootDirectory, item.IconPath + ".png");

                if (File.Exists(fullPath))
                {
                    try
                    {
                        var icon = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
                        _loadedIcons.Add(icon);
                        Console.WriteLine($"Loaded icon: {item.Name}");
                    }
                    catch
                    {
                        _loadedIcons.Add(null);
                        Console.WriteLine($"Failed to load icon for: {item.Name}");
                    }
                }
                else
                {
                    _loadedIcons.Add(null);
                    Console.WriteLine($"Icon file not found: {fullPath}");
                }
            }

            // Grid layout (4 columns x 2 rows)
            int index = 0;
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    _inventoryRects[index] = new Rectangle(
                        220 + col * 230,
                        340 + row * 210,
                        160,
                        160
                    );
                    index++;
                }
            }

            _backButtonRect = new Rectangle(1500, 80, 300, 80);
        }

        public void SetSelectedSlot(int slot, Screen previousScreen)
        {
            _selectedSlot = slot;
            _previousScreen = previousScreen;
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Game.Window.Title = $"DevGame - Equipment Screen - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        public override void OnMouseClick(Point mousePos)
        {
            if (_backButtonRect.Contains(mousePos))
            {
                ReturnToPrevious();
                return;
            }

            for (int i = 0; i < _inventoryItems.Count && i < _inventoryRects.Length; i++)
            {
                if (
                    _inventoryRects[i].Contains(mousePos)
                    && _selectedSlot >= 0
                    && Game.Player != null
                )
                {
                    Game.Player.EquipItem(_selectedSlot, _inventoryItems[i]);
                    Console.WriteLine(
                        $"Equipped '{_inventoryItems[i].Name}' to slot {_selectedSlot + 1}"
                    );
                    ReturnToPrevious();
                    return;
                }
            }
        }

        private void ReturnToPrevious()
        {
            if (_previousScreen != null)
                Game.PushScreen(_previousScreen);
            else
                Game.ChangeState(GameState.MainHub);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(20, 15, 40));

            if (_font != null)
            {
                spriteBatch.DrawString(
                    _font,
                    "EQUIPMENT MANAGEMENT",
                    new Vector2(680, 80),
                    Color.White
                );

                if (_selectedSlot >= 0)
                    spriteBatch.DrawString(
                        _font,
                        $"Equipping to Slot {_selectedSlot + 1}",
                        new Vector2(200, 160),
                        Color.Yellow
                    );

                spriteBatch.DrawString(
                    _font,
                    "AVAILABLE ITEMS - Click to equip",
                    new Vector2(200, 260),
                    Color.Cyan
                );

                for (int i = 0; i < _inventoryItems.Count && i < _inventoryRects.Length; i++)
                {
                    var rect = _inventoryRects[i];
                    spriteBatch.Draw(_whitePixel, rect, new Color(55, 50, 80));

                    if (i < _loadedIcons.Count && _loadedIcons[i] != null)
                    {
                        spriteBatch.Draw(
                            _loadedIcons[i]!,
                            new Rectangle(rect.X + 10, rect.Y + 10, 140, 140),
                            Color.White
                        );
                    }
                    else
                    {
                        spriteBatch.DrawString(
                            _font,
                            "NO ICON",
                            new Vector2(rect.X + 40, rect.Y + 70),
                            Color.Red
                        );
                    }

                    spriteBatch.DrawString(
                        _font,
                        _inventoryItems[i].Name,
                        new Vector2(rect.X + 10, rect.Y + 170),
                        Color.LightGray
                    );
                }

                spriteBatch.Draw(_whitePixel, _backButtonRect, new Color(100, 30, 30));
                spriteBatch.DrawString(
                    _font,
                    "BACK TO PREVIOUS",
                    new Vector2(1530, 105),
                    Color.White
                );

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}

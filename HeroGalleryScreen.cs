// HeroGalleryScreen.cs - Improved icon loading with better debugging
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedRPG
{
    public class HeroGalleryScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        private Rectangle[] _heroRects = new Rectangle[8];

        // Cache for hero class icons
        private Dictionary<string, Texture2D?> _heroIconCache =
            new Dictionary<string, Texture2D?>();

        public HeroGalleryScreen(Game1 game)
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

            // Load hero icons
            LoadHeroIcons();

            // Create grid (2 rows × 4 columns)
            int index = 0;
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    _heroRects[index] = new Rectangle(180 + col * 380, 180 + row * 380, 280, 320);
                    index++;
                }
            }
        }

        private void LoadHeroIcons()
        {
            // Match your actual filenames exactly
            var classToFile = new Dictionary<string, string>
            {
                { "brawler", "brawler" },
                { "sharpshooter", "sharpshooter" },
                { "healer", "healer" },
                { "magician", "magician" },
                { "jack of all trades", "jack_of_all_trades" },
                { "jack-of-all-trades", "jack_of_all_trades" },
                { "player", "player_male" }, // fallback
            };

            foreach (var entry in classToFile)
            {
                string fullPath = Path.Combine(
                    Game.Content.RootDirectory,
                    "Characters/Icons",
                    entry.Value + ".png"
                );

                if (File.Exists(fullPath))
                {
                    try
                    {
                        var icon = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
                        _heroIconCache[entry.Key] = icon;
                        Console.WriteLine($"HeroGallery: Successfully loaded {entry.Value}.png");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"HeroGallery: Failed to load {entry.Value}.png → {ex.Message}"
                        );
                        _heroIconCache[entry.Key] = null;
                    }
                }
                else
                {
                    Console.WriteLine($"HeroGallery: File not found → {fullPath}");
                    _heroIconCache[entry.Key] = null;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Game.Window.Title = $"DevGame - Hero Gallery - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        public override void OnMouseClick(Point mousePos)
        {
            int displayedIndex = 0;

            for (int i = 0; i < Game.ActiveParty.Count; i++)
            {
                var hero = Game.ActiveParty[i];
                if (hero is PlayerHero)
                    continue;

                if (
                    displayedIndex < _heroRects.Length
                    && _heroRects[displayedIndex].Contains(mousePos)
                )
                {
                    Game.ShowHeroDetail(hero);
                    return;
                }

                displayedIndex++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(18, 15, 38));

            if (_font != null)
            {
                spriteBatch.DrawString(_font, "HERO GALLERY", new Vector2(780, 80), Color.White);

                int displayedIndex = 0;

                for (int i = 0; i < Game.ActiveParty.Count; i++)
                {
                    var hero = Game.ActiveParty[i];
                    if (hero is PlayerHero)
                        continue;

                    if (displayedIndex >= _heroRects.Length)
                        break;

                    var rect = _heroRects[displayedIndex];
                    spriteBatch.Draw(_whitePixel, rect, new Color(45, 40, 75));

                    // Get correct class key
                    string classKey = hero.Class.ToLower().Replace(" ", "_").Replace("-", "_");

                    if (_heroIconCache.TryGetValue(classKey, out var icon) && icon != null)
                    {
                        spriteBatch.Draw(
                            icon,
                            new Rectangle(rect.X + 60, rect.Y + 40, 160, 160),
                            Color.White
                        );
                    }
                    else
                    {
                        // Fallback
                        spriteBatch.Draw(
                            _whitePixel,
                            new Rectangle(rect.X + 60, rect.Y + 40, 160, 160),
                            new Color(60, 55, 85)
                        );
                        spriteBatch.DrawString(
                            _font,
                            "NO ICON",
                            new Vector2(rect.X + 95, rect.Y + 110),
                            Color.Red
                        );
                    }

                    spriteBatch.DrawString(
                        _font,
                        hero.Name,
                        new Vector2(rect.X + 40, rect.Y + 220),
                        Color.White
                    );
                    spriteBatch.DrawString(
                        _font,
                        $"({hero.Class})",
                        new Vector2(rect.X + 40, rect.Y + 255),
                        Color.Cyan
                    );

                    displayedIndex++;
                }

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}

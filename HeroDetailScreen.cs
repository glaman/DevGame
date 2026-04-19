// HeroDetailScreen.cs - Fixed constructor + proper icon loading
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace TurnBasedRPG
{
    public class HeroDetailScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;
        private Hero? _currentHero;

        private Texture2D? _heroIcon;

        private Rectangle _backButtonRect;

        // Constructor - only takes Game1 (matches what Game1.cs expects)
        public HeroDetailScreen(Game1 game) : base(game)
        {
        }

        // Method to set the hero to display (called from Game1.ShowHeroDetail)
        public void SetHero(Hero hero)
        {
            _currentHero = hero;
            LoadHeroIcon();
        }

        private void LoadHeroIcon()
        {
            _heroIcon = null;
            if (_currentHero == null) return;

            string iconPath = GetIconPathForClass(_currentHero.Class);

            string fullPath = Path.Combine(Game.Content.RootDirectory, iconPath + ".png");

            if (File.Exists(fullPath))
            {
                try
                {
                    _heroIcon = Texture2D.FromFile(Game.GraphicsDevice, fullPath);
                    Console.WriteLine($"Loaded hero icon for: {_currentHero.Class}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load hero icon {_currentHero.Class}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Hero icon file not found: {fullPath}");
            }
        }

        private string GetIconPathForClass(string heroClass)
        {
            return heroClass.ToLower() switch
            {
                "brawler" => "Characters/Icons/brawler",
                "sharpshooter" => "Characters/Icons/sharpshooter",
                "healer" => "Characters/Icons/healer",
                "magician" => "Characters/Icons/magician",
                "jack of all trades" or "jack-of-all-trades" => "Characters/Icons/jack_of_all_trades",
                _ => "Characters/Icons/player_male"  // fallback
            };
        }

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

            _backButtonRect = new Rectangle(1500, 80, 300, 80);
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Game.Window.Title = $"DevGame - Hero Detail - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        protected override void OnMouseClick(Point mousePos)
        {
            if (_backButtonRect.Contains(mousePos))
            {
                //Game.HandleEscape(); // Smart back / exit behavior
                Game.ChangeState(GameState.MainHub);
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(15, 20, 45));

            if (_font != null && _currentHero != null)
            {
                // Large hero portrait area
                var portraitRect = new Rectangle(180, 180, 520, 520);
                spriteBatch.Draw(_whitePixel, portraitRect, new Color(40, 35, 70));

                if (_heroIcon != null)
                {
                    spriteBatch.Draw(_heroIcon, new Rectangle(portraitRect.X + 40, portraitRect.Y + 40, 440, 440), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(_font, "NO ICON", new Vector2(portraitRect.X + 180, portraitRect.Y + 240), Color.Red);
                }

                // Hero information
                spriteBatch.DrawString(_font, _currentHero.Name.ToUpper(), new Vector2(820, 220), Color.White);
                spriteBatch.DrawString(_font, _currentHero.Class, new Vector2(820, 280), Color.Cyan);

                // Stats
                int y = 380;
                spriteBatch.DrawString(_font, $"Might     : {_currentHero.Might}", new Vector2(820, y), Color.LightGray); y += 45;
                spriteBatch.DrawString(_font, $"Finesse   : {_currentHero.Finesse}", new Vector2(820, y), Color.LightGray); y += 45;
                spriteBatch.DrawString(_font, $"Wit       : {_currentHero.Wit}", new Vector2(820, y), Color.LightGray); y += 45;
                spriteBatch.DrawString(_font, $"Vigor     : {_currentHero.Vigor}", new Vector2(820, y), Color.LightGray); y += 45;
                spriteBatch.DrawString(_font, $"Speed     : {_currentHero.Speed}", new Vector2(820, y), Color.LightGray);

                // Back button
                spriteBatch.Draw(_whitePixel, _backButtonRect, new Color(100, 30, 30));
                spriteBatch.DrawString(_font, "BACK TO GALLERY", new Vector2(1530, 105), Color.White);

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}

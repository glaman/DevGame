// BattleScreen.cs - Refined layout with Guard/Defend ability included
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TurnBasedRPG
{
    public class BattleScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        // Top: Turn Order (max 100px height)
        private Rectangle _turnOrderRect;
        private Rectangle[] _turnOrderUnitRects = new Rectangle[8];

        // Left: Player Team - H / P / H formation
        private Rectangle _playerTeamRect;
        private Rectangle _helper1Rect;   // Top helper
        private Rectangle _playerRect;    // Center player (leader)
        private Rectangle _helper2Rect;   // Bottom helper

        // Right: Enemies (up to 6)
        private Rectangle _enemySideRect;
        private Rectangle[] _enemyRects = new Rectangle[6];

        // Bottom Left: Abilities (25% height, 50% width)
        private Rectangle _actionBarRect;
        private Rectangle[] _actionRects = new Rectangle[5]; // 4 abilities + Guard/Defend

        // Bottom Right: Action Log (25% height, 50% width)
        private Rectangle _actionLogRect;

        public BattleScreen(Game1 game) : base(game)
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

            // Turn Order
            _turnOrderRect = new Rectangle(0, 0, 1920, 100);
            for (int i = 0; i < 8; i++)
            {
                _turnOrderUnitRects[i] = new Rectangle(200 + i * 110, 10, 90, 80);
            }

            // Player Team - H / P / H formation
            _playerTeamRect = new Rectangle(80, 140, 420, 520);
            _helper1Rect = new Rectangle(140, 160, 180, 160);
            _playerRect   = new Rectangle(120, 360, 220, 220);   // Larger leader position
            _helper2Rect  = new Rectangle(140, 620, 180, 160);

            // Enemies
            _enemySideRect = new Rectangle(580, 140, 1260, 520);
            for (int i = 0; i < 6; i++)
            {
                int col = i % 3;
                int row = i / 3;
                _enemyRects[i] = new Rectangle(640 + col * 280, 180 + row * 260, 240, 220);
            }

            // Bottom Left: Abilities + Guard/Defend (5 slots)
            _actionBarRect = new Rectangle(80, 720, 860, 280);
            for (int i = 0; i < 5; i++)
            {
                _actionRects[i] = new Rectangle(140 + i * 160, 760, 140, 200);
            }

            // Bottom Right: Action Log
            _actionLogRect = new Rectangle(980, 720, 860, 280);
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            Game.Window.Title = $"DevGame - Battle Screen - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        protected override void OnMouseClick(Point mousePosition)
        {
            // Click on abilities or Guard/Defend
            for (int i = 0; i < 5; i++)
            {
                if (_actionRects[i].Contains(mousePosition))
                {
                    if (i == 4)
                        Console.WriteLine("Guard/Defend selected - Reducing incoming damage this round");
                    else
                        Console.WriteLine($"Ability {i + 1} selected");
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(15, 12, 28));

            if (_font != null)
            {
                // Top: Turn Order
                spriteBatch.Draw(_whitePixel, _turnOrderRect, new Color(35, 30, 55));
                spriteBatch.DrawString(_font, "TURN ORDER", new Vector2(80, 25), Color.Cyan);

                for (int i = 0; i < 8; i++)
                {
                    spriteBatch.Draw(_whitePixel, _turnOrderUnitRects[i], new Color(55, 50, 80));
                    spriteBatch.DrawString(_font, $"U{i+1}", new Vector2(_turnOrderUnitRects[i].X + 30, _turnOrderUnitRects[i].Y + 25), Color.LightGray);
                }

                // Left: Player Team - H / P / H formation
                spriteBatch.Draw(_whitePixel, _playerTeamRect, new Color(38, 33, 68));
                spriteBatch.DrawString(_font, "YOUR TEAM", new Vector2(140, 150), Color.Cyan);

                // Helper 1 (top)
                spriteBatch.Draw(_whitePixel, _helper1Rect, new Color(50, 45, 75));
                spriteBatch.DrawString(_font, "H1", new Vector2(190, 200), Color.LightGray);

                // Player (center, larger, leader)
                spriteBatch.Draw(_whitePixel, _playerRect, new Color(65, 55, 95));
                spriteBatch.DrawString(_font, "P\n(YOU)", new Vector2(190, 420), Color.Yellow);

                // Helper 2 (bottom)
                spriteBatch.Draw(_whitePixel, _helper2Rect, new Color(50, 45, 75));
                spriteBatch.DrawString(_font, "H2", new Vector2(190, 660), Color.LightGray);

                // Right: Enemies
                spriteBatch.Draw(_whitePixel, _enemySideRect, new Color(70, 25, 25));
                spriteBatch.DrawString(_font, "ENEMIES", new Vector2(1080, 150), Color.Red);

                for (int i = 0; i < 6; i++)
                {
                    spriteBatch.Draw(_whitePixel, _enemyRects[i], new Color(90, 35, 35));
                    spriteBatch.DrawString(_font, $"E{i+1}", new Vector2(_enemyRects[i].X + 90, _enemyRects[i].Y + 90), Color.LightGray);
                }

                // Bottom Left: Abilities + Guard/Defend
                spriteBatch.Draw(_whitePixel, _actionBarRect, new Color(28, 24, 50));
                spriteBatch.DrawString(_font, "ACTIONS / ABILITIES", new Vector2(140, 740), Color.Cyan);

                // 4 normal abilities + Guard/Defend as 5th option
                for (int i = 0; i < 4; i++)
                {
                    spriteBatch.Draw(_whitePixel, _actionRects[i], new Color(60, 55, 90));
                    spriteBatch.DrawString(_font, $"Ability {i+1}", new Vector2(_actionRects[i].X + 20, _actionRects[i].Y + 80), Color.LightGray);
                }

                // Guard / Defend (always available)
                spriteBatch.Draw(_whitePixel, _actionRects[4], new Color(45, 70, 45)); // Green tint for defensive action
                spriteBatch.DrawString(_font, "GUARD / DEFEND", new Vector2(_actionRects[4].X + 10, _actionRects[4].Y + 70), Color.LightGreen);
                spriteBatch.DrawString(_font, "(Reduce damage this round)", new Vector2(_actionRects[4].X + 10, _actionRects[4].Y + 110), Color.DarkGray);

                // Bottom Right: Action Log
                spriteBatch.Draw(_whitePixel, _actionLogRect, new Color(28, 24, 50));
                spriteBatch.DrawString(_font, "BATTLE LOG", new Vector2(1020, 740), Color.Cyan);
                spriteBatch.DrawString(_font, "Turn 1: Waiting for player action...", 
                    new Vector2(1020, 800), Color.LightGray);

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}

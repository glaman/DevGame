// TitleScreen.cs - Super safe version (only basic characters)
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TurnBasedRPG
{
    public class TitleScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        public TitleScreen(Game1 game) : base(game)
        {
        }

        public override void LoadContent()
        {
            _whitePixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new Color[] { Color.White });

            try
            {
                _font = Game.Content.Load<SpriteFont>("DefaultFont");
                Console.WriteLine("TitleScreen: Font loaded successfully.");
            }
            catch (Exception ex)
            {
                _font = null;
                Console.WriteLine("TitleScreen: Font failed - " + ex.Message);
            }
        }
public override void Update(GameTime gameTime)
{
    HandleInput(gameTime);

    Game.Window.Title = "DevGame - Title Screen - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    var input = Game.InputManager;

    if (input.IsKeyJustPressed(Keys.Enter) || input.IsKeyJustPressed(Keys.N))
    {
        Console.WriteLine("Starting New Player creation");
        Game.ChangeState(GameState.PlayerProfile);
    }

    if (input.IsKeyJustPressed(Keys.C))
    {
        Console.WriteLine("Opening Save Slot Selection");
        Game.ChangeState(GameState.SaveSlotSelection);   // New state
    }
}

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), Color.DarkSlateBlue);

            if (_font != null)
            {
                spriteBatch.DrawString(_font, "DEVGAME", 
                    new Vector2(760, 220), Color.White);

                spriteBatch.DrawString(_font, "Turn Based RPG", 
                    new Vector2(780, 300), Color.LightGray);

                spriteBatch.DrawString(_font, "Press N or ENTER to Start New Hero", 
                    new Vector2(620, 480), Color.LightGreen);

                spriteBatch.DrawString(_font, "Press C to Continue Existing Hero", 
                    new Vector2(620, 540), Color.LightBlue);

                DrawDevMenu(spriteBatch, _font);
            }
            else
            {
                spriteBatch.Draw(_whitePixel, new Rectangle(500, 200, 920, 140), Color.DarkBlue);
            }
        }
    }
}

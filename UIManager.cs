// UIManager.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedRPG
{
    public class UIManager
    {
        private Game1 _game;
        private Screen? _currentScreen;

        public Screen? CurrentScreen => _currentScreen;

        public UIManager(Game1 game)
        {
            _game = game;
        }

        public void ChangeScreen(Screen newScreen)
        {
            _currentScreen = newScreen;
            _currentScreen?.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(spriteBatch);
        }
    }
}

// Screen.cs - Fixed for mouse click forwarding from Game1
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public abstract class Screen
    {
        protected Game1 Game { get; private set; }

        protected Screen(Game1 game)
        {
            Game = game;
        }

        public virtual void LoadContent() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        // ====================== INPUT ======================
        public void HandleInput(GameTime gameTime)
        {
            HandleDevNavigation(gameTime);
            HandleMouseInput();
        }

        // Dev keyboard navigation
        protected virtual void HandleDevNavigation(GameTime gameTime)
        {
            var input = Game.InputManager;
            if (input.IsKeyJustPressed(Keys.T))
                Game.ChangeState(GameState.Title);
            if (input.IsKeyJustPressed(Keys.D))
                Game.ChangeState(GameState.MainHub);
            if (input.IsKeyJustPressed(Keys.M))
                Game.ChangeState(GameState.Map);
            if (input.IsKeyJustPressed(Keys.B))
                Game.ChangeState(GameState.Battle);
            if (input.IsKeyJustPressed(Keys.E))
                Game.ChangeState(GameState.Equipment);
            if (input.IsKeyJustPressed(Keys.H))
                Game.ChangeState(GameState.HeroGallery);
            if (input.IsKeyJustPressed(Keys.P))
                Game.ChangeState(GameState.PlayerProfile);

            // S = Save, L = Load
            if (input.IsKeyJustPressed(Keys.S))
            {
                Game.SaveSlotSelectionScreen.SetMode(true); // Save mode
                Game.ChangeState(GameState.SaveSlotSelection);
            }
            if (input.IsKeyJustPressed(Keys.L))
            {
                Game.SaveSlotSelectionScreen.SetMode(false); // Load mode
                Game.ChangeState(GameState.SaveSlotSelection);
            }

            if (input.IsKeyJustPressed(Keys.F4))
                Game.Exit();

            if (input.IsKeyJustPressed(Keys.Escape))
                Game.HandleEscape();
        }

        // Mouse handling
        protected virtual void HandleMouseInput()
        {
            if (Game.InputManager.IsLeftMouseJustPressed())
            {
                Point mousePos = Game.InputManager.GetMousePosition();
                OnMouseClick(mousePos);
            }
        }

        // Made PUBLIC so Game1.cs can call it directly
        public virtual void OnMouseClick(Point mousePosition)
        {
            // Default: do nothing. Derived screens override this.
        }

        // Dev menu at bottom
        protected virtual void DrawDevMenu(SpriteBatch spriteBatch, SpriteFont? font)
        {
            if (font == null)
                return;

            string devText =
                "T=Title D=Dashboard P=Profile M=Map B=Battle E=Equipment H=Heroes S=Save L=Load F4=Exit ESC=Back";
            using var barTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            barTexture.SetData(new Color[] { new Color(0, 0, 0, 180) });
            spriteBatch.Draw(barTexture, new Rectangle(0, 1020, 1920, 60), Color.White);
            spriteBatch.DrawString(font, devText, new Vector2(40, 1035), Color.Cyan);
        }
    }
}

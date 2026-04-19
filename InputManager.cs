// InputManager.cs - With mouse support
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class InputManager
    {
        private KeyboardState _currentKeyboard;
        private KeyboardState _previousKeyboard;
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        public void Update()
        {
            _previousKeyboard = _currentKeyboard;
            _previousMouse = _currentMouse;

            _currentKeyboard = Keyboard.GetState();
            _currentMouse = Mouse.GetState();
        }

        public bool IsKeyJustPressed(Keys key)
        {
            return _currentKeyboard.IsKeyDown(key) && !_previousKeyboard.IsKeyDown(key);
        }

        public bool IsLeftMouseJustPressed()
        {
            return _currentMouse.LeftButton == ButtonState.Pressed && 
                   _previousMouse.LeftButton == ButtonState.Released;
        }

        public Point GetMousePosition()
        {
            return _currentMouse.Position;
        }

        public MouseState GetMouseState()
        {
            return _currentMouse;
        }
    }
}

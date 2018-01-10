using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PokemonTcg.Assets;

namespace PokemonTcg
{
    public static class InputManager
    {
        private static KeyboardState currentKeyboardState;
        private static KeyboardState oldKeyboardState;
        private static MouseState currentMouseState;
        private static MouseState oldMouseState;

        public static void Update()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            var mousePos = currentMouseState.Position;
            
            var realX = (1920f / Settings.ClientWidth) * currentMouseState.X;
            var realY = (1080f / Settings.ClientHeight) * currentMouseState.Y;
            MousePosition = new Point((int)realX, (int)realY);
        }

        public static Point MousePosition { get; private set; }

        public static bool IsKeyPressed(Keys key)
        {
            return oldKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsLeftMousePressed()
        {
            return oldMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightMousePressed()
        {
            return oldMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsMiddleMousePressed()
        {
            return oldMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed;
        }
    }
}

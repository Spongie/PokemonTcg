using Microsoft.Xna.Framework.Input;

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
        }

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

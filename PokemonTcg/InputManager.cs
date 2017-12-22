using Microsoft.Xna.Framework.Input;

namespace PokemonTcg
{
    public static class InputManager
    {
        private static KeyboardState currentState;
        private static KeyboardState oldState;

        public static void Update()
        {
            oldState = currentState;
            currentState = Keyboard.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return oldState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }
    }
}

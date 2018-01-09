using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTcg.Rendering
{
    public struct DelayedDraw
    {
        public DelayedDraw(Texture2D texture, Rectangle rectangle)
        {
            Rectangle = rectangle;
            Texture = texture;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        public Texture2D Texture { get; }
        public Rectangle Rectangle { get; }
    }
}

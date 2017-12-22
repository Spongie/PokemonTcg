using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TCGCards.Core;

namespace PokemonTcg
{
    public class PokemonGame
    {
        private Player player;
        private GameField playingField;

        private SpriteFont defaultFont;

        public PokemonGame(ContentManager content)
        {
            playingField = new GameField();
            player = playingField.ActivePlayer;

            defaultFont = content.Load<SpriteFont>("Fonts/Default");
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyPressed(Keys.D))
                player.DrawCards(1);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            int y = 20;
            foreach(var card in player.Hand)
            {
                spriteBatch.DrawString(defaultFont, card.GetName(), new Vector2(10, y), Color.Black);
                y += 25;
            }
        }
    }
}

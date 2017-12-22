using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using TCGCards;
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

            if(InputManager.IsKeyPressed(Keys.P))
                player.SetActivePokemon((IPokemonCard)player.Hand.First(x => x is IPokemonCard));
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(defaultFont, "Hand: ", new Vector2(10, 5), Color.Black);
            int y = 25;
            foreach(var card in player.Hand)
            {
                spriteBatch.DrawString(defaultFont, card.GetName(), new Vector2(10, y), Color.Black);
                y += 25;
            }

            spriteBatch.DrawString(defaultFont, "Active pokemon: ", new Vector2(200, 5), Color.Black);
            if (player.ActivePokemonCard != null)
            {
                spriteBatch.DrawString(defaultFont, player.ActivePokemonCard.GetName(), new Vector2(200, 25), Color.Black);
            }
        }
    }
}

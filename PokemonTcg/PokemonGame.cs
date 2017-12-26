using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkingClient;
using NetworkingServer;
using System;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TCGCards.PokemonCards.TeamRocket;

namespace PokemonTcg
{
    public class PokemonGame
    {
        private Player player;
        private GameField playingField;
        private SpriteFont defaultFont;
        private Server server;
        private Client client;

        public PokemonGame(ContentManager content)
        {
            defaultFont = content.Load<SpriteFont>("Fonts/Default");

            client = new Client();
            client.OnGameUpdated += Client_OnGameUpdated;
            client.Start("127.0.0.1", 1000);
        }

        private void Client_OnGameUpdated(object sender, GameUpdatedEventArgs e)
        {
            playingField = e.Game;
            player = playingField.Players.FirstOrDefault(p => p.Id == client.Player.Id);
        }

        public void Update(GameTime gameTime)
        {
            if(playingField == null || playingField.GameState == GameFieldState.WaitingForConnection)
                return;

            if (playingField.GameState == GameFieldState.WaitingForRegistration && !client.RegistrationSent)
            {
                var testDeck = new Deck();
                testDeck.Cards.Push(new Magikarp());
                testDeck.Cards.Push(new Magikarp());
                testDeck.Cards.Push(new Magikarp());
                testDeck.Cards.Push(new Magikarp());
                testDeck.Cards.Push(new Magikarp());
                testDeck.Cards.Push(new Magikarp());
                testDeck.Cards.Push(new WaterEnergy());
                testDeck.Cards.Push(new FireEnergy());
                testDeck.Cards.Push(new FireEnergy());
                testDeck.Cards.Push(new WaterEnergy());
                testDeck.Cards.Push(new WaterEnergy());
                testDeck.Cards.Push(new WaterEnergy());

                client.Register("Name", testDeck);
            }

            if(playingField.ActivePlayer.Id == player.Id)
            {
                if(InputManager.IsKeyPressed(Keys.D))
                    player.DrawCards(1);

                if(InputManager.IsKeyPressed(Keys.P))
                    player.SetActivePokemon((IPokemonCard)player.Hand.First(x => x is IPokemonCard));
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            if (playingField != null)
            {
                spriteBatch.DrawString(defaultFont, Enum.GetName(typeof(GameFieldState), playingField.GameState), new Vector2(10, 5), Color.Black);
                spriteBatch.DrawString(defaultFont, "Connected Players: " + playingField.Players.Count, new Vector2(250, 5), Color.Black);
            }

            if(player != null)
            {
                var activeString = player.Id == playingField.ActivePlayer.Id ? "My turn" : "Opponents turn";

                spriteBatch.DrawString(defaultFont, activeString, new Vector2(450, 5), Color.Black);
                spriteBatch.DrawString(defaultFont, "Hand: ", new Vector2(10, 35), Color.Black);
                int y = 55;
                foreach(var card in player.Hand)
                {
                    spriteBatch.DrawString(defaultFont, card.GetName(), new Vector2(10, y), Color.Black);
                    y += 25;
                }

                spriteBatch.DrawString(defaultFont, "Active pokemon: ", new Vector2(200, 35), Color.Black);
                if(player.ActivePokemonCard != null)
                {
                    spriteBatch.DrawString(defaultFont, player.ActivePokemonCard.GetName(), new Vector2(200, 55), Color.Black);
                }
            }
        }
    }
}

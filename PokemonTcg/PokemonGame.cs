using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkingClient;
using NetworkingServer;
using PokemonTcg.Assets;
using PokemonTcg.Rendering;
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
        private TextureManager textureManager;
        private Point middleClickPoint;
        private PlayerRenderer playerRenderer;
        private PlayerRenderer opponentRenderer;
        private Player opponent;

        public PokemonGame(ContentManager content)
        {
            defaultFont = content.Load<SpriteFont>("Fonts/Default");
            textureManager = new TextureManager(content);

            //client = new Client();
            //client.OnGameUpdated += Client_OnGameUpdated;
            //client.Start("127.0.0.1", 1000);

            player = new Player();
            player.Hand.Add(new Magikarp(player));
            player.Hand.Add(new Magikarp(player));
            player.Hand.Add(new Magikarp(player));
            player.Hand.Add(new DarkGyarados(player));
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());

            player.SetActivePokemon((IPokemonCard)player.Hand.First());

            player.BenchedPokemon.Add(new Magikarp(player));
            player.BenchedPokemon.Add(new Magikarp(player));
            player.BenchedPokemon.Add(new Magikarp(player));
            player.BenchedPokemon.Add(new Magikarp(player));
            player.BenchedPokemon.Add(new Magikarp(player));

            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));
            player.Deck.Cards.Push(new Magikarp(player));

            playerRenderer = PlayerRenderer.CreateForPlayer(player);

            opponent = new Player();
            opponent.Hand.Add(new Magikarp(player));
            opponent.Hand.Add(new Magikarp(player));
            opponent.Hand.Add(new Magikarp(player));
            opponent.Hand.Add(new DarkGyarados(player));
            opponent.Hand.Add(new WaterEnergy());
            opponent.Hand.Add(new WaterEnergy());
            opponent.Hand.Add(new WaterEnergy());
            opponent.Hand.Add(new WaterEnergy());

            opponent.SetActivePokemon((IPokemonCard)player.Hand.First());

            opponent.BenchedPokemon.Add(new Magikarp(player));
            opponent.BenchedPokemon.Add(new Magikarp(player));
            opponent.BenchedPokemon.Add(new Magikarp(player));
            opponent.BenchedPokemon.Add(new Magikarp(player));
            opponent.BenchedPokemon.Add(new Magikarp(player));

            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));
            opponent.Deck.Cards.Push(new Magikarp(player));

            opponentRenderer = PlayerRenderer.CreateForOpponent(opponent);
            playerRenderer.Add(opponentRenderer);
        }

        private void Client_OnGameUpdated(object sender, GameUpdatedEventArgs e)
        {
            playingField = e.Game;
            player = playingField.Players.FirstOrDefault(p => p.Id == client.Player.Id);
        }

        public void Update(GameTime gameTime)
        {
            if(InputManager.IsKeyPressed(Keys.D))
                player.DrawCards(1);

            if (InputManager.IsMiddleMousePressed())
                middleClickPoint = InputManager.MousePosition;

            //opponentRenderer.Update();
            playerRenderer.Update();
            

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
            //opponentRenderer.Render(spriteBatch);
            playerRenderer.Render(spriteBatch);

            spriteBatch.DrawString(defaultFont, "X: " + InputManager.MousePosition.X, new Vector2(20, 15), Color.Black);
            spriteBatch.DrawString(defaultFont, "Y: " + InputManager.MousePosition.Y, new Vector2(20, 30), Color.Black);

            spriteBatch.Draw(textureManager.LoadCardTexture(new WaterEnergy()), new Rectangle(0, 1080 / 2, 1920, 1), Color.Black);
            spriteBatch.Draw(textureManager.LoadCardTexture(new WaterEnergy()), new Rectangle(1920 / 2, 0, 1, 1080), Color.Black);
        }

        private void RenderTestText(SpriteBatch spriteBatch)
        {
            if(playingField != null)
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

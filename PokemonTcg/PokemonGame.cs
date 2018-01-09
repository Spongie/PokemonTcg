using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkingClient;
using NetworkingServer;
using PokemonTcg.Assets;
using PokemonTcg.Rendering;
using System;
using System.Collections.Generic;
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
            player.Hand.Add(new DarkGyarados(player));
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());

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
                middleClickPoint = Mouse.GetState().Position;

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
            RenderBench(spriteBatch);
            RenderPlayerHand(spriteBatch);
            
            //RenderTestText(spriteBatch);
        }

        private const int benchCardWidth = 125;
        private const int benchCardHeight = 150;
        private const int benchCardSpacing = 10;

        private void RenderBench(SpriteBatch spriteBatch)
        {
            var cardRectangle = new Rectangle(600, 650, benchCardWidth, benchCardHeight);
            var delayedRenders = new List<DelayedDraw>();

            foreach(var card in player.BenchedPokemon)
            {
                if(cardRectangle.Contains(Mouse.GetState().Position))
                    delayedRenders.Add(new DelayedDraw(textureManager.LoadCardTexture(card), new Rectangle((int)(cardRectangle.X - benchCardWidth / 2.5f), (int)(cardRectangle.Y - ((benchCardHeight * 2.5f) - benchCardHeight)), (int)(benchCardWidth * 2.5f), (int)(benchCardHeight * 2.5f))));
                else
                    spriteBatch.Draw(textureManager.LoadCardTexture(card), cardRectangle, Color.PaleVioletRed);

                cardRectangle.X += benchCardWidth + benchCardSpacing;
            }

            foreach(var drawing in delayedRenders)
            {
                drawing.Render(spriteBatch);
            }
        }

        private const int cardWidth = 150;
        private const int cardWidthSpacing = 10;
        private const int cardHeight = 250;

        private void RenderPlayerHand(SpriteBatch spriteBatch)
        {
            var totalCardsize = (cardWidth + cardWidthSpacing) * player.Hand.Count;
            var totalEmptySpace = 1920 - totalCardsize;

            var cardRectangle = new Rectangle(totalEmptySpace / 2, 820, cardWidth, cardHeight);

            var delayedRenders = new List<DelayedDraw>();

            foreach(var card in player.Hand)
            {
                if(cardRectangle.Contains(Mouse.GetState().Position))
                    delayedRenders.Add(new DelayedDraw(textureManager.LoadCardTexture(card), new Rectangle(cardRectangle.X - cardWidth / 2, cardRectangle.Y - cardHeight, cardWidth * 2, cardHeight * 2)));
                else
                    spriteBatch.Draw(textureManager.LoadCardTexture(card), cardRectangle, Color.White);

                cardRectangle.X += cardWidth + cardWidthSpacing;
            }

            foreach(var drawing in delayedRenders)
            {
                drawing.Render(spriteBatch);
            }
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

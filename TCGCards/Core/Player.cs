using Assets.Scripts.Cards;
using Assets.Scripts.Cards.EnergyCards;
using Assets.Scripts.Cards.PokemonCards.TeamRocket;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
    public class Player
    {
        public Player()
        {
            Deck = new Deck();
            Deck.Cards.Add(new WaterEnergy());
            Deck.Cards.Add(new WaterEnergy());
            Deck.Cards.Add(new WaterEnergy());
            Deck.Cards.Add(new WaterEnergy());
            Deck.Cards.Add(new Magikarp());
            Deck.Cards.Add(new Magikarp());
            Deck.Cards.Add(new Magikarp());
            Deck.Cards.Add(new Magikarp());
            Deck.Shuffle();
            Hand = new List<ICard>();
            DrawCards(5);
        }

        public void PlayCard(ICard card)
        {

        }

        public void DrawCards(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                Hand.Add(Deck.DrawCard());
            }
        }

        public List<IPokemonCard> BenchedPokemon { get; set; }

        public IPokemonCard ActivePokemonCard { get; set; }

        public List<ICard> PrizeCards { get; set; }

        public List<ICard> DiscardPile { get; set; }

        public Deck Deck { get; set; }

        public List<ICard> Hand { get; set; }
    }
}

using Assets.Scripts.Cards;
using Assets.Scripts.Cards.EnergyCards;
using Assets.Scripts.Cards.PokemonCards.TeamRocket;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
    public class Player
    {
        private Deck deck;
        private List<ICard> discardPile;
        private List<ICard> prizeCards;
        private IPokemonCard activePokemonCard;
        private List<IPokemonCard> benchedPokemon;
        private List<ICard> _Hand;

        public Player()
        {
            deck = new Deck();
            deck.Cards.Add(new WaterEnergy());
            deck.Cards.Add(new WaterEnergy());
            deck.Cards.Add(new WaterEnergy());
            deck.Cards.Add(new WaterEnergy());
            deck.Cards.Add(new Magikarp());
            deck.Cards.Add(new Magikarp());
            deck.Cards.Add(new Magikarp());
            deck.Cards.Add(new Magikarp());
            deck.Shuffle();
            _Hand = new List<ICard>();
            DrawCards(5);
        }

        public void PlayCard(ICard card)
        {

        }

        public void DrawCards(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                _Hand.Add(Deck.DrawCard());
            }
        }

        public List<IPokemonCard> BenchedPokemon
        {
            get
            {
                return benchedPokemon;
            }

            set
            {
                benchedPokemon = value;
            }
        }

        public IPokemonCard ActivePokemonCard
        {
            get
            {
                return activePokemonCard;
            }

            set
            {
                activePokemonCard = value;
            }
        }

        public List<ICard> PrizeCards
        {
            get
            {
                return prizeCards;
            }

            set
            {
                prizeCards = value;
            }
        }

        public List<ICard> DiscardPile
        {
            get
            {
                return discardPile;
            }

            set
            {
                discardPile = value;
            }
        }

        public Deck Deck
        {
            get
            {
                return deck;
            }

            set
            {
                deck = value;
            }
        }

        public List<ICard> Hand
        {
            get
            {
                return _Hand;
            }

            set
            {
                _Hand = value;
            }
        }
    }
}

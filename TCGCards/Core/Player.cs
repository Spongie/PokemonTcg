using System.Collections.Generic;
using TCGCards.EnergyCards;
using TCGCards.PokemonCards.TeamRocket;

namespace TCGCards.Core
{
    public class Player
    {
        public Player()
        {
            Deck = new Deck();
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new Magikarp());
            Deck.Cards.Push(new Magikarp());
            Deck.Cards.Push(new Magikarp());
            Deck.Cards.Push(new Magikarp());
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
                if (Deck.Cards.Count > 0)
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

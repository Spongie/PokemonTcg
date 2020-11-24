using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    [Serializable]
    public class Deck
    {
        public Deck()
        {
            Cards = new Stack<Card>();
        }

        public Stack<Card> Cards { get; set; }

        public void Shuffle()
        {
            var random = new Random();
            Cards = new Stack<Card>(Cards.OrderBy(x => random.Next(10000)));
        }

        public Card DrawCard()
        {
            return Cards.Pop();
        }

        public void ShuffleInCard(Card card)
        {
            Cards.Push(card);
            Shuffle();
        }

        public void ShuffleInCards(List<Card> cards)
        {
            foreach (var card in cards)
            {
                Cards.Push(card);
            }

            Shuffle();
        }
    }
}

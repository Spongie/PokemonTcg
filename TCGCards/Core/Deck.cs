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
            Cards = new Stack<ICard>();
        }

        public Stack<ICard> Cards { get; set; }

        public void Shuffle()
        {
            var random = new Random();
            Cards = new Stack<ICard>(Cards.OrderBy(x => random.Next(10000)));
        }

        public ICard DrawCard()
        {
            return Cards.Pop();
        }
    }
}

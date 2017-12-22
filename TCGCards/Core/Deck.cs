using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    [Serializable]
    public class Deck
    {
        private Stack<ICard> _Cards;

        public Deck()
        {
            _Cards = new Stack<ICard>();
        }

        public Stack<ICard> Cards
        {
            get
            {
                return _Cards;
            }

            set
            {
                _Cards = value;
            }
        }

        public void Shuffle()
        {
            var random = new Random();
            Cards = new Stack<ICard>(Cards.OrderBy(x => random.Next(10000)));
        }

        public ICard DrawCard()
        {
            return _Cards.Pop();
        }
    }
}

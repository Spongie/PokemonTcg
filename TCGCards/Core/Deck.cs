using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Game
{
    [Serializable]
    public class Deck
    {
        private List<ICard> _Cards;

        public Deck()
        {
            _Cards = new List<ICard>();
        }

        public List<ICard> Cards
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
            Cards = Cards.OrderBy(x => random.Next(10000)).ToList();
        }

        public ICard DrawCard()
        {
            return _Cards[0];
        }
    }
}

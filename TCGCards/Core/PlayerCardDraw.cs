using System.Collections.Generic;

namespace TCGCards.Core
{
    public class PlayerCardDraw
    {
        public int Amount { get; set; }
        public Player Player { get; set; }
        public List<Card> Cards { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TCGCards.Core
{
    public class DeckSearchEventHandler : EventArgs
    {
        public DeckSearchEventHandler(Player player, List<IDeckFilter> filters, int cardCount)
        {
            Player = player;
            Filters = filters;
            CardCount = cardCount;
        }

        public Player Player { get; }
        public List<IDeckFilter> Filters { get; }
        public int CardCount { get; }
    }
}
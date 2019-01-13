using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DeckSearchMessage : AbstractNetworkMessage
    {
        public DeckSearchMessage(Player player, List<IDeckFilter> filters, int cardCount)
        {
            Player = player;
            Filters = filters;
            CardCount = cardCount;
            PickedCards = new List<Card>();
            MessageType = MessageTypes.DeckSearch;
        }

        public Player Player { get; }
        public List<IDeckFilter> Filters { get; }
        public int CardCount { get; }
        public List<Card> PickedCards { get; set; }
    }
}

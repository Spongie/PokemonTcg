using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class PickFromListMessage : AbstractNetworkMessage
    {
        private List<ICard> prizeCards;

        public PickFromListMessage(List<ICard> possibleChoices, List<IDeckFilter> filter, int cardCount)
        {
            PossibleChoices = possibleChoices;
            Filter = filter;
            CardCount = cardCount;
        }

        public PickFromListMessage(List<ICard> possibleChoices, IDeckFilter filter, int cardCount)
            : this(possibleChoices, new List<IDeckFilter> { filter }, cardCount)
        {
        }

        public List<ICard> PossibleChoices { get; }
        public List<IDeckFilter> Filter { get; }
        public int CardCount { get; }
    }
}

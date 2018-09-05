using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class PickFromListMessage : AbstractNetworkMessage
    {
        public PickFromListMessage(List<Card> possibleChoices, List<IDeckFilter> filter, int cardCount)
        {
            PossibleChoices = possibleChoices;
            Filter = filter;
            CardCount = cardCount;
        }

        public PickFromListMessage(List<Card> possibleChoices, IDeckFilter filter, int cardCount)
            : this(possibleChoices, new List<IDeckFilter> { filter }, cardCount)
        {
        }

        public List<Card> PossibleChoices { get; }
        public List<IDeckFilter> Filter { get; }
        public int CardCount { get; }
    }
}

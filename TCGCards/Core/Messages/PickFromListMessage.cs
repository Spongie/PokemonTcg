using NetworkingCore;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Messages
{
    public class PickFromListMessage : AbstractNetworkMessage
    {
        public PickFromListMessage(IEnumerable<Card> possibleChoices, List<IDeckFilter> filter, int cardCount)
        {
            MessageType = MessageTypes.PickFromList;
            PossibleChoices = possibleChoices.ToList();
            Filter = filter;
            CardCount = cardCount;
        }

        public PickFromListMessage(IEnumerable<Card> possibleChoices, IDeckFilter filter, int cardCount)
            : this(possibleChoices, new List<IDeckFilter> { filter }, cardCount)
        {
        }

        public List<Card> PossibleChoices { get; }
        public List<IDeckFilter> Filter { get; }
        public int CardCount { get; }
    }
}

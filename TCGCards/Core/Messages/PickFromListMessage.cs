using NetworkingCore;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Messages
{
    public class PickFromListMessage : AbstractNetworkMessage
    {
        public PickFromListMessage(IEnumerable<Card> possibleChoices, int cardCount)
        {
            MessageType = MessageTypes.PickFromList;
            PossibleChoices = possibleChoices.ToList();
            CardCount = cardCount;
        }

        public List<Card> PossibleChoices { get; }
        public int CardCount { get; }
    }
}

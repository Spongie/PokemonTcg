using NetworkingCore;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Messages
{
    public class PickFromListMessage : AbstractNetworkMessage
    {
        public PickFromListMessage()
        {

        }

        public PickFromListMessage(IEnumerable<Card> possibleChoices, int count) : this(possibleChoices, count, count)
        {

        }

        public PickFromListMessage(IEnumerable<Card> possibleChoices, int minCount, int maxCount)
        {
            MessageType = MessageTypes.PickFromList;
            PossibleChoices = possibleChoices.ToList();
            MaxCount = maxCount;
            MinCount = minCount;
        }

        public List<Card> PossibleChoices { get; }
        public int MaxCount { get; }
        public int MinCount { get; }
    }
}

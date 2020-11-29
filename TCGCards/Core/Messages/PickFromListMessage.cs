using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class PickFromListMessage : AbstractNetworkMessage
    {
        public PickFromListMessage() :this(new List<Card>(), 1)
        {

        }

        public PickFromListMessage(List<Card> possibleChoices, int count) : this(possibleChoices, count, count)
        {

        }

        public PickFromListMessage(List<Card> possibleChoices, int minCount, int maxCount)
        {
            MessageType = MessageTypes.PickFromList;
            PossibleChoices = new List<Card>(possibleChoices);
            MaxCount = maxCount;
            MinCount = minCount;
        }

        public List<Card> PossibleChoices { get; set; }
        public int MaxCount { get; set; }
        public int MinCount { get; set; }
    }
}

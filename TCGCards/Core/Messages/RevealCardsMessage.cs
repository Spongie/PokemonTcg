using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class RevealCardsMessage : AbstractNetworkMessage
    {
        public RevealCardsMessage() :this(new List<Card>())
        {
            
        }

        public RevealCardsMessage(List<Card> cards)
        {
            MessageType = MessageTypes.RevealCardsMessage;
            Cards = cards;
        }

        public List<Card> Cards { get; set; }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            return new NetworkMessage(MessageType, this, senderId, NetworkId.Generate()) { RequiresResponse = false };
        }
    }
}

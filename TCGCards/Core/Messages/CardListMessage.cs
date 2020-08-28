using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class CardListMessage : AbstractNetworkMessage
    {
        public CardListMessage()
        {

        }

        public CardListMessage(List<NetworkId> cards)
        {
            Cards = new List<NetworkId>(cards);
            MessageType = MessageTypes.CardListMessage;
        }

        public List<NetworkId> Cards { get; set; }
    }
}

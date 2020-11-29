using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectPrizeCardsMessage : AbstractNetworkMessage
    {
        public SelectPrizeCardsMessage()
        {

        }

        public SelectPrizeCardsMessage(int amount)
        {
            Amount = amount;
            MessageType = MessageTypes.SelectPrizeCards;
        }

        public int Amount { get; set; }
    }
}

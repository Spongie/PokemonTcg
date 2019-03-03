using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectPriceCardsMessage : AbstractNetworkMessage
    {
        public SelectPriceCardsMessage(int amount)
        {
            Amount = amount;
            MessageType = MessageTypes.SelectPriceCards;
        }

        public int Amount { get; }
    }
}

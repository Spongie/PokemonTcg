using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DeckSearchedMessage : AbstractNetworkMessage
    {
        public DeckSearchedMessage(List<Card> selectedCards, Player owner)
        {
            SelectedCards = selectedCards;
            Owner = owner;
            messageType = MessageTypes.SelectedActive;
        }

        public List<Card> SelectedCards { get; set; }
        public Player Owner { get; set; }
    }
}

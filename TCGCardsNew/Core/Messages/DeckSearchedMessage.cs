using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DeckSearchedMessage : AbstractNetworkMessage
    {
        public DeckSearchedMessage(List<ICard> selectedCards, Player owner)
        {
            SelectedCards = selectedCards;
            Owner = owner;
            messageType = MessageTypes.SelectedActive;
        }

        public List<ICard> SelectedCards { get; set; }
        public Player Owner { get; set; }
    }
}

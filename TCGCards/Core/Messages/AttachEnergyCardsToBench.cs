using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class AttachEnergyCardsToBenchMessage : AbstractNetworkMessage
    {
        public AttachEnergyCardsToBenchMessage()
        {

        }

        public AttachEnergyCardsToBenchMessage(List<EnergyCard> energyCards)
        {
            MessageType = MessageTypes.AttachEnergyToBench;
            EnergyCards = new List<EnergyCard>(energyCards);
        }

        public List<EnergyCard> EnergyCards { get; set; }
    }
}

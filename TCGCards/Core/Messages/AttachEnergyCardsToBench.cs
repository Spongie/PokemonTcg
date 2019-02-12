using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class AttachEnergyCardsToBenchMessage : AbstractNetworkMessage
    {
        public AttachEnergyCardsToBenchMessage(List<EnergyCard> energyCards)
        {
            MessageType = MessageTypes.AttachEnergyToBench;
            EnergyCards = energyCards;
        }

        public List<EnergyCard> EnergyCards { get; set; }
    }

    public class AttachedEnergyDoneMessage : AbstractNetworkMessage
    {
        public AttachedEnergyDoneMessage(Dictionary<NetworkId, NetworkId> energyPokemonMap)
        {
            EnergyPokemonMap = energyPokemonMap;
        }

        public Dictionary<NetworkId, NetworkId> EnergyPokemonMap { get; set; }
    }
}

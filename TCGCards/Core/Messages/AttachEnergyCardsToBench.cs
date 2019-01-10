using NetworkingCore;
using System;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class AttachEnergyCardsToBenchMessage : AbstractNetworkMessage
    {
        public AttachEnergyCardsToBenchMessage(List<EnergyCard> energyCards)
        {
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

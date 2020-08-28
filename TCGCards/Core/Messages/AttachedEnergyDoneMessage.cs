using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class AttachedEnergyDoneMessage : AbstractNetworkMessage
    {
        public AttachedEnergyDoneMessage()
        {

        }

        public AttachedEnergyDoneMessage(Dictionary<NetworkId, NetworkId> energyPokemonMap)
        {
            EnergyPokemonMap = energyPokemonMap;
        }

        public Dictionary<NetworkId, NetworkId> EnergyPokemonMap { get; set; }
    }
}

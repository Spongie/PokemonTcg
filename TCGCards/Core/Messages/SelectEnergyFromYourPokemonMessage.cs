using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class SelectEnergyFromPokemonMessage : AbstractNetworkMessage
    {
        public SelectEnergyFromPokemonMessage()
        {
            MessageType = MessageTypes.SelectEnergyFromPokemon;
        }

        public int Amount { get; set; }
        public IDeckFilter Filter { get; set; }
        public List<PokemonCard> Pokemons { get; set; }
    }
}

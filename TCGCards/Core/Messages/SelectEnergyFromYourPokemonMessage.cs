using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class SelectEnergyFromYourPokemonMessage : AbstractNetworkMessage
    {
        public int Amount { get; set; }
        public IDeckFilter Filter { get; set; }
        public List<PokemonCard> Pokemons { get; set; }
    }
}

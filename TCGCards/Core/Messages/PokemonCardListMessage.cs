using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class PokemonCardListMessage : AbstractNetworkMessage
    {
        public PokemonCardListMessage(List<IPokemonCard> pokemons)
        {
            Pokemons = new List<IPokemonCard>(pokemons);
        }

        public List<IPokemonCard> Pokemons { get; set; }
    }
}

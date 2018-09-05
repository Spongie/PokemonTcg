using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class PokemonCardListMessage : AbstractNetworkMessage
    {
        public PokemonCardListMessage(List<PokemonCard> pokemons)
        {
            Pokemons = new List<PokemonCard>(pokemons);
        }

        public List<PokemonCard> Pokemons { get; set; }
    }
}

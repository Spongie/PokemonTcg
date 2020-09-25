using PokemonTcgSdk.Models;
using System.Collections.Generic;

namespace CardEditor.Models
{
    public class JsonPokemon
    {
        public PokemonCard Card { get; set; }
    }

    public class JsonPokemonList
    {
        public List<PokemonCard> Cards { get; set; }
    }
}

using PokemonTcgSdk;
using System.Collections.Generic;

namespace CardEditor.Models
{
    public class JsonEnergyCard
    {
        public Energy Card { get; set; }
    }

    public class JsonEnergyList
    {
        public List<Energy> Cards { get; set; }
    }
}

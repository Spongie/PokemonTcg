using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Mewtwo : PokemonCard
    {
        public Mewtwo(Player owner) : base(owner)
        {
            PokemonName = "Mewtwo";
			
            Hp = 60;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Psychic(),
				new Barrier()
            };
			
        }
    }
}

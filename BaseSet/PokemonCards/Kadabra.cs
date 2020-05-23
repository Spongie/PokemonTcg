using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Kadabra : PokemonCard
    {
        public Kadabra(Player owner) : base(owner)
        {
            PokemonName = "Kadabra";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 60;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Recover(),
				new SuperPsy()
            };
			
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Raticate : PokemonCard
    {
        public Raticate(Player owner) : base(owner)
        {
            PokemonName = "Raticate";
			EvolvesFrom = PokemonNames.Rattata;
            Hp = 60;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Bite(),
				new SuperFang()
            };
			
        }
    }
}

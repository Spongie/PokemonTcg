using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Dugtrio : PokemonCard
    {
        public Dugtrio(Player owner) : base(owner)
        {
            PokemonName = "Dugtrio";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Diglett;
            Hp = 70;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 2;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.Electric;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Slash(),
				new Earthquake()
            };
			
        }
    }
}

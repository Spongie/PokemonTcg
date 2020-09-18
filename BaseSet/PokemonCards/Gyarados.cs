using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Gyarados : PokemonCard
    {
        public Gyarados(Player owner) : base(owner)
        {
            PokemonName = "Gyarados";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Magikarp;
            Hp = 100;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 3;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.Fighting;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new DragonRage(),
				new Bubblebeam()
            };
			
        }
    }
}

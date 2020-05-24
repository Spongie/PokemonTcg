using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Pidgeotto : PokemonCard
    {
        public Pidgeotto(Player owner) : base(owner)
        {
            PokemonName = "Pidgeotto";
			EvolvesFrom = PokemonNames.Pidgey;
            Hp = 60;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.Fighting;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Whirlwind(),
				new MirrorMove()
            };
			
        }
    }
}

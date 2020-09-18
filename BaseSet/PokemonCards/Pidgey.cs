using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Pidgey : PokemonCard
    {
        public Pidgey(Player owner) : base(owner)
        {
            PokemonName = "Pidgey";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.Fighting;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Whirlwind()
            };
			
        }
    }
}

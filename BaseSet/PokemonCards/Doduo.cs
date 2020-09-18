using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Doduo : PokemonCard
    {
        public Doduo(Player owner) : base(owner)
        {
            PokemonName = "Doduo";
            Set = Singleton.Get<Set>();
            Hp = 50;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 0;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.Fighting;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new FuryAttack()
            };
			
        }
    }
}

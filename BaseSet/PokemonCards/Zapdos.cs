using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Zapdos : PokemonCard
    {
        public Zapdos(Player owner) : base(owner)
        {
            PokemonName = "Zapdos";
            Set = Singleton.Get<Set>();
            Hp = 90;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 3;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Fighting;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Thunder(),
				new Thunderbolt()
            };
			
        }
    }
}

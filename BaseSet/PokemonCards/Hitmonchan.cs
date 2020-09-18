using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Hitmonchan : PokemonCard
    {
        public Hitmonchan(Player owner) : base(owner)
        {
            PokemonName = "Hitmonchan";
            Set = Singleton.Get<Set>();
            Hp = 70;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Jab(),
				new SpecialPunch()
            };
			
        }
    }
}

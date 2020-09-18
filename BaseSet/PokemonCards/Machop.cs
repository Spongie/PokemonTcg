using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Machop : PokemonCard
    {
        public Machop(Player owner) : base(owner)
        {
            PokemonName = "Machop";
            Set = Singleton.Get<Set>();
            Hp = 50;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new LowKick()
            };
			
        }
    }
}

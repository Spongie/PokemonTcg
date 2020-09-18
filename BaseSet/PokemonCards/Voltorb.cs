using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Voltorb : PokemonCard
    {
        public Voltorb(Player owner) : base(owner)
        {
            PokemonName = "Voltorb";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Tackle()
            };
			
        }
    }
}

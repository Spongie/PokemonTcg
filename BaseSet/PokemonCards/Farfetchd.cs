using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Farfetchd : PokemonCard
    {
        public Farfetchd(Player owner) : base(owner)
        {
            PokemonName = "Farfetch'd";
			
            Hp = 50;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.Fighting;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new LeekSlap(),
				new PotSmash()
            };
			
        }
    }
}

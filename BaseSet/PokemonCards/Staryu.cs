using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Staryu : PokemonCard
    {
        public Staryu(Player owner) : base(owner)
        {
            PokemonName = "Staryu";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Slap()
            };
			
        }
    }
}

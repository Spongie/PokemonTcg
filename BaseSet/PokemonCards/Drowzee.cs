using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Drowzee : PokemonCard
    {
        public Drowzee(Player owner) : base(owner)
        {
            PokemonName = "Drowzee";
            Set = Singleton.Get<Set>();
            Hp = 50;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 0;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Pound(),
				new ConfuseRay()
            };
			
        }
    }
}

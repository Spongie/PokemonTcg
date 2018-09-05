using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Magnemite : PokemonCard
    {
        public Magnemite(Player owner) : base(owner)
        {
            PokemonName = "Magnemite";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Tackle(),
				new Magnetism()
            };
			
        }
    }
}

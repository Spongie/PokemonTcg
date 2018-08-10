using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Mankey : IPokemonCard
    {
        public Mankey(Player owner) : base(owner)
        {
            PokemonName = "Mankey";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 0;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Mischief(),
				new Anger()
            };
			
        }
    }
}

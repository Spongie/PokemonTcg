using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Dratini : IPokemonCard
    {
        public Dratini(Player owner) : base(owner)
        {
            PokemonName = "Dratini";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
				new Wrap()
            };
			
        }
    }
}

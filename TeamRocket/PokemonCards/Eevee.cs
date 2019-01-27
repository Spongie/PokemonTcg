using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Eevee : PokemonCard
    {
        public Eevee(Player owner) : base(owner)
        {
            PokemonName = "Eevee";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
				new Tackle10(),
				new Sandattack()
            };
			
        }
    }
}

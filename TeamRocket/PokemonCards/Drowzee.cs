using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Drowzee : PokemonCard
    {
        public Drowzee(Player owner) : base(owner)
        {
            PokemonName = "Drowzee";
			Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Nightmare()
            };
			//TODO: Pokemon power
        }
    }
}

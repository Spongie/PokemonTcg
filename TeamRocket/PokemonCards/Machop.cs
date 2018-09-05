using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Machop : PokemonCard
    {
        public Machop(Player owner) : base(owner)
        {
            PokemonName = "Machop";
			Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Punch(),
				new Kick()
            };
			
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Diglett : PokemonCard
    {
        public Diglett(Player owner) : base(owner)
        {
            PokemonName = "Diglett";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 0;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.Electric;
            Attacks = new List<Attack>
            {
				new DigUnder(),
				new Scratch()
            };
			
        }
    }
}

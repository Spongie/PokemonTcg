using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Jynx : PokemonCard
    {
        public Jynx(Player owner) : base(owner)
        {
            PokemonName = "Jynx";
			
            Hp = 70;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Doubleslap(),
				new Meditate()
            };
			
        }
    }
}

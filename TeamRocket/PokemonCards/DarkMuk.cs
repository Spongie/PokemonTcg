using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMuk : IPokemonCard
    {
        public DarkMuk(Player owner) : base(owner)
        {
            PokemonName = "Dark Muk";
			EvolvesFrom = "Grimer"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new SludgePunch()
            };
			//TODO: Pokemon power
        }
    }
}

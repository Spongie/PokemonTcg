using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkDragonite : IPokemonCard
    {
        public DarkDragonite(Player owner) : base(owner)
        {
            PokemonName = "Dark Dragonite";
			EvolvesFrom = "Dark Dragonair"; //TODO: Add stage
            Hp = 70;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 2;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Fighting;
            Attacks = new List<Attack>
            {
				new GiantTail()
            };
			//TODO: Pokemon power
        }
    }
}

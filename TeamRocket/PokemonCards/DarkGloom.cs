using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGloom : IPokemonCard
    {
        public DarkGloom(Player owner) : base(owner)
        {
            PokemonName = "Dark Gloom";
			EvolvesFrom = "Oddish"; //TODO: Add stage
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Poisonpowder()
            };
			//TODO: Pokemon power
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkVileplume : IPokemonCard
    {
        public DarkVileplume(Player owner) : base(owner)
        {
            PokemonName = "Dark Vileplume";
			EvolvesFrom = "Dark Gloom"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new PetalWhirlwind()
            };
			//TODO: Pokemon power
        }
    }
}

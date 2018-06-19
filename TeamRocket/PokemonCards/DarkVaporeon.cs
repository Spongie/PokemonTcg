using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkVaporeon : IPokemonCard
    {
        public DarkVaporeon(Player owner) : base(owner)
        {
            PokemonName = "Dark Vaporeon";
			EvolvesFrom = "Eevee"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Bite(),
				new Whirlpool()
            };
			
        }
    }
}

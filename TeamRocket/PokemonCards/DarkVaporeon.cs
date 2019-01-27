using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkVaporeon : PokemonCard
    {
        public DarkVaporeon(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkVaporeon;
            EvolvesFrom = PokemonNames.Eevee;
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Bite30(),
				new Whirlpool()
            };
			
        }
    }
}

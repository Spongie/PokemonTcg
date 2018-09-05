using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGolduck : PokemonCard
    {
        public DarkGolduck(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkGolduck;
			EvolvesFrom = PokemonNames.Psyduck;
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 2;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new ThirdEye(),
				new SuperPsy()
            };
			
        }
    }
}

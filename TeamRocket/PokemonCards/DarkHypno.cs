using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkHypno : PokemonCard
    {
        public DarkHypno(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkHypno;
			EvolvesFrom = PokemonNames.Drowzee;
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Psypunch(),
				new BenchManipulation()
            };
			
        }
    }
}

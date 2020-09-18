using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMachamp : PokemonCard
    {
        public DarkMachamp(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkMachamp;
			EvolvesFrom = PokemonNames.DarkMachoke;
            Stage = 2;
            Hp = 70;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new MegaPunch(),
				new Fling()
            };
			
        }
    }
}

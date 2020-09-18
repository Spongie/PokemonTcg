using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Dragonair : PokemonCard
    {
        public Dragonair(Player owner) : base(owner)
        {
            PokemonName = "Dragonair";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Dratini;
            Hp = 80;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 2;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Psychic;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Slam(),
				new HyperBeam()
            };
			
        }
    }
}

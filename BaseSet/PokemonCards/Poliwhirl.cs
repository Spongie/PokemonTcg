using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Poliwhirl : PokemonCard
    {
        public Poliwhirl(Player owner) : base(owner)
        {
            PokemonName = "Poliwhirl";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Poliwag;
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Amnesia(),
				new Doubleslap()
            };
			
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Arcanine : PokemonCard
    {
        public Arcanine(Player owner) : base(owner)
        {
            PokemonName = "Arcanine";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 100;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 3;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Flamethrower(),
				new TakeDown()
            };
			
        }
    }
}

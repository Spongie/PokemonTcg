using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Venusaur : PokemonCard
    {
        public Venusaur(Player owner) : base(owner)
        {
            PokemonName = "Venusaur";
			EvolvesFrom = PokemonNames.Ivysaur;
            Hp = 100;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new Solarbeam()
            };
			//TODO: Pokemon power
        }
    }
}

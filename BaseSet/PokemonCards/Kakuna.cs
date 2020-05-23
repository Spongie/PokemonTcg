using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Kakuna : PokemonCard
    {
        public Kakuna(Player owner) : base(owner)
        {
            PokemonName = "Kakuna";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 80;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Stiffen(),
				new Poisonpowder()
            };
			
        }
    }
}

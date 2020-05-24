using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Ninetales : PokemonCard
    {
        public Ninetales(Player owner) : base(owner)
        {
            PokemonName = "Ninetales";
			EvolvesFrom = PokemonNames.Vulpix;
            Hp = 80;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Lure(),
				new FireBlast()
            };
			
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Charmeleon : PokemonCard
    {
        public Charmeleon(Player owner) : base(owner)
        {
            PokemonName = "Charmeleon";
			EvolvesFrom = PokemonNames.Charmander;
            Hp = 80;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Slash(),
				new Flamethrower()
            };
			
        }
    }
}

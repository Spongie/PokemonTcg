using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;
using TCGCards.Core.Abilities;

namespace BaseSet.PokemonCards
{
    public class Blastoise : PokemonCard
    {
        public Blastoise(Player owner) : base(owner)
        {
            PokemonName = "Blastoise";
			EvolvesFrom = PokemonNames.Wartortle;
            Hp = 100;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 3;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new HydroPump()
            };
            Ability = new AttachEnergyAbility(this, new[] { EnergyTypes.Water }, new[] { EnergyTypes.Water }, 9999);
        }
    }
}

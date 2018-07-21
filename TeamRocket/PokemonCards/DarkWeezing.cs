using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkWeezing : IPokemonCard
    {
        public DarkWeezing(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkWeezing;
			EvolvesFrom = PokemonNames.Koffing;
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new MassExplosion(),
				new StunGas()
            };
			
        }
    }
}

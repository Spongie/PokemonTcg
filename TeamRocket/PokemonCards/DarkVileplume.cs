using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkVileplume : PokemonCard
    {
        public DarkVileplume(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkVileplume;
			EvolvesFrom = PokemonNames.DarkGloom;
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new PetalWhirlwind()
            };
			//TODO: Pokemon power
        }
    }
}

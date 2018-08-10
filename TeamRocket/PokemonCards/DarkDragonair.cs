using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkDragonair : IPokemonCard
    {
        public DarkDragonair(Player owner) : base(owner)
        {
            PokemonName = "Dark Dragonair";
			EvolvesFrom = "Dratini";
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 2;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
				new TailStrike()
            };
			//TODO: Pokemon power
        }
    }
}

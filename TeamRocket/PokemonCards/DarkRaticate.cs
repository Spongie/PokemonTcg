using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkRaticate : PokemonCard
    {
        public DarkRaticate(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkRaticate;
			EvolvesFrom = PokemonNames.Rattata;
            Stage = 1;
            Hp = 50;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
				new Gnaw(),
				new HyperFang()
            };
			
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkRaichu : IPokemonCard
    {
        public DarkRaichu(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkRaichu;
			EvolvesFrom = PokemonNames.Pikachu;
            Stage = 1;
            Hp = 70;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new SurpriseThunder()
            };
			
        }
    }
}

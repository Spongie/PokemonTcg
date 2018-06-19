using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkCharizard : IPokemonCard
    {
        public DarkCharizard(Player owner) : base(owner)
        {
            PokemonName = "Dark Charizard";
			EvolvesFrom = "Dark Charmeleon"; //TODO: Add stage
            Hp = 80;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 3;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.Fighting;
            Attacks = new List<Attack>
            {
				new NailFlick(),
				new ContinuousFireball()
            };
			
        }
    }
}

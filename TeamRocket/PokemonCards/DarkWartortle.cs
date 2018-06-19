using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkWartortle : IPokemonCard
    {
        public DarkWartortle(Player owner) : base(owner)
        {
            PokemonName = "Dark Wartortle";
			EvolvesFrom = "Squirtle"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Doubleslap(),
				new MirrorShell()
            };
			
        }
    }
}

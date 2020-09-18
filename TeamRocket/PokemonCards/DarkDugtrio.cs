using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkDugtrio : PokemonCard
    {
        public DarkDugtrio(Player owner) : base(owner)
        {
            PokemonName = "Dark Dugtrio";
			EvolvesFrom = "Diglett";
            Set = Singleton.Get<Set>();
            Stage = 1;
            Hp = 50;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 2;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.Electric;
            Attacks = new List<Attack>
            {
				new KnockDown()
            };
            Ability = new Sinkhole(this);
        }
    }
}

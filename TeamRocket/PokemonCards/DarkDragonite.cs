using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkDragonite : PokemonCard
    {
        public DarkDragonite(Player owner) : base(owner)
        {
            PokemonName = "Dark Dragonite";
			EvolvesFrom = "Dark Dragonair";
            Set = Singleton.Get<Set>();
            Stage = 2;
            Hp = 70;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 2;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Fighting;
            Attacks = new List<Attack>
            {
				new GiantTail()
            };
            Ability = new SummonMinions(this);
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMuk : PokemonCard
    {
        public DarkMuk(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkMuk;
			EvolvesFrom = PokemonNames.Grimer;
            Stage = 1;
            Hp = 60;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new SludgePunch()
            };
            Ability = new StickyGoo(this);
        }
    }
}

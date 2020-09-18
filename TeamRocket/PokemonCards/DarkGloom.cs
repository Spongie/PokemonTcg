using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGloom : PokemonCard
    {
        public DarkGloom(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkGloom;
			EvolvesFrom = PokemonNames.Oddish;
            Set = Singleton.Get<Set>();
            Stage = 1;
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new PoisonpowderDarkGloom()
            };
            Ability = new PollenStench(this);
        }
    }
}

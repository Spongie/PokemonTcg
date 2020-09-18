using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkPrimeape : PokemonCard
    {
        public DarkPrimeape(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkPrimeape;
			EvolvesFrom = PokemonNames.Mankey;
            Stage = 1;
            Hp = 60;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new FrenziedAttack()
            };
            Ability = new Frenzy(this);
        }
    }
}

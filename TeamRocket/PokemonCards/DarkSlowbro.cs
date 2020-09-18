using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkSlowbro : PokemonCard
    {
        public DarkSlowbro(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkSlowbro;
			EvolvesFrom = PokemonNames.Slowpoke;
            Stage = 1;
            Hp = 60;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new FickleAttack()
            };
            Ability = new ReelIn(this);
        }
    }
}

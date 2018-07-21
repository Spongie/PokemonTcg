using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkSlowbro : IPokemonCard
    {
        public DarkSlowbro(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkSlowbro;
			EvolvesFrom = PokemonNames.Slowpoke;
            Stage = 1;
            Hp = 60;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new FickleAttack()
            };
			//TODO: Pokemon power
        }
    }
}

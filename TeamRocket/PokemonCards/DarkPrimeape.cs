using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkPrimeape : IPokemonCard
    {
        public DarkPrimeape(Player owner) : base(owner)
        {
            PokemonName = "Dark Primeape";
			EvolvesFrom = "Mankey"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new FrenziedAttack()
            };
			//TODO: Pokemon power
        }
    }
}

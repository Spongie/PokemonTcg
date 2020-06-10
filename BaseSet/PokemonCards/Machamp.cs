using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;
using TCGCards.Core.Abilities;

namespace BaseSet.PokemonCards
{
    public class Machamp : PokemonCard
    {
        public Machamp(Player owner) : base(owner)
        {
            PokemonName = "Machamp";
			EvolvesFrom = PokemonNames.Machoke;
            Hp = 100;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new SeismicToss()
            };
            Ability = new TakesDamagesOnAttacked(this, 10)
            {
                Name = "Strikes Back",
                Description = "Whenever your opponent's attack damages Machamp (even if Machamp is Knoced Out), this power does 10 damage to attacking Pokémon. (Don't apply Weakness and Resistance.) This power can't be used if Machamp is already Asleep, Confused, or Paralyzed when your opponent attacks."
            };
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Squirtle : IPokemonCard
    {
        protected Squirtle(Player owner) : base(owner)
        {
            Hp = 50;
            PokemonType = EnergyTypes.Water;
            Weakness = EnergyTypes.Electric;
            Stage = 0;
            RetreatCost = 1;
            PokemonName = PokemonNames.Squirtle;
            Attacks = new List<Attack>
            {
                new ShellAttack()
            };
        }
    }
}

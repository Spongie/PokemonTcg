using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Squirtle : PokemonCard
    {
        protected Squirtle(Player owner) : base(owner)
        {
            Hp = 50;
            PokemonType = EnergyTypes.Water;
            Weakness = EnergyTypes.Electric;
            Stage = 0;
            RetreatCost = 1;
            Set = Singleton.Get<Set>();
            PokemonName = PokemonNames.Squirtle;
            Attacks = new List<Attack>
            {
                new ShellAttack()
            };
        }
    }
}

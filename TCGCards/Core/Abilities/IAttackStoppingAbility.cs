using System;
using System.Collections.Generic;
using System.Text;

namespace TCGCards.Core.Abilities
{
    public interface IAttackStoppingAbility
    {
        bool IsStopped(GameField game, PokemonCard attacker, PokemonCard defender);
    }
}

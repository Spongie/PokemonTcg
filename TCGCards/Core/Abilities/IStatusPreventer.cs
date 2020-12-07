using Entities;

namespace TCGCards.Core.Abilities
{
    public interface IStatusPreventer
    {
        bool PreventsEffect(StatusEffect statusEffect, GameField game);
    }
}
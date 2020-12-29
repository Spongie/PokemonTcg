using TCGCards.Core;

namespace TCGCards.TrainerEffects.Util
{
    public interface IEffectCondition
    {
        bool IsOk(GameField game, Player caster);
    }
}

using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public interface ITrainerEffect
    {
        void Process(GameField game, Player caster, Player opponent);
        bool CanCast(GameField game, Player caster, Player opponent);
        string EffectType { get; }
    }
}

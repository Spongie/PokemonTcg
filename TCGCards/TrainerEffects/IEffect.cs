using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public interface IEffect
    {
        void Process(GameField game, Player caster, Player opponent);
        void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game);
        bool CanCast(GameField game, Player caster, Player opponent);
        string EffectType { get; }
    }
}

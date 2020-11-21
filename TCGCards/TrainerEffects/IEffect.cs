using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public interface IEffect
    {
        void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource);
        void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game);
        bool CanCast(GameField game, Player caster, Player opponent);
        string EffectType { get; }
    }
}

namespace TCGCards.Core.Abilities
{
    public interface IAttackStoppingAbility
    {
        bool IsStopped(GameField game, PokemonCard attacker, PokemonCard defender);
    }
}

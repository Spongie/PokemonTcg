namespace TCGCards.Core.Abilities
{
    public interface IDamageTakenModifier
    {
        int GetModifiedDamage(int damageTaken, GameField game);
    }
}

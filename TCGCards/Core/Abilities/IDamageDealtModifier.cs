namespace TCGCards.Core.Abilities
{
    public interface IDamageDealtModifier
    {
        int GetModifiedDamage(int damageDone, GameField game);
    }
}

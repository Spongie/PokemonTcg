namespace TCGCards.Core
{
    public abstract class WorldAbility
    {
        public int TurnsLeft { get; set; }
        public WorldAbilityTriggerType TriggerType { get; set; }
        public bool Triggered { get; set; }

        public abstract void Activate(GameField gameField, Damage currentDamage);
    }
}

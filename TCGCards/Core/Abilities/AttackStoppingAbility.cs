namespace TCGCards.Core.Abilities
{
    public class AttackStoppingAbility : PassiveAbility
    {
        public AttackStoppingAbility() :base(null)
        {
            ModifierType = PassiveModifierType.StopAttack;
        }
    }
}

using NetworkingCore;

namespace TCGCards.Core.Abilities
{
    public class AttackStoppingAbility : PassiveAbility
    {
        public AttackStoppingAbility() :base(null)
        {
            ModifierType = PassiveModifierType.StopAttack;
        }

        public NetworkId OnlyOnCard { get; set; }
        public string OnlyStopThisAttack { get; set; }
    }
}

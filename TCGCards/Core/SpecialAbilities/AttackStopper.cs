using System;

namespace TCGCards.Core.SpecialAbilities
{
    public class AttackStopper : TimedSpecialAbility
    {
        private Func<bool> action;

        public AttackStopper(Func<bool> action) :this(action, 2)
        {

        }

        public AttackStopper(Func<bool> action, int turnDuration) : base(turnDuration)
        {
            this.action = action;
        }

        public bool IsAttackIgnored() => action.Invoke();
    }
}

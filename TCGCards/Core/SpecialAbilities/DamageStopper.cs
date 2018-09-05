using System;

namespace TCGCards.Core.SpecialAbilities
{
    public class DamageStopper : TimedSpecialAbility
    {
        private Func<bool> action;

        public DamageStopper(Func<bool> action) : this(action, 2)
        {

        }

        public DamageStopper(Func<bool> action, int turnDuration) : base(turnDuration)
        {
            this.action = action;
        }

        public bool IsDamageIgnored() => action.Invoke();
    }
}

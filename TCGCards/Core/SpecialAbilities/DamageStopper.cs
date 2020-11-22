using System;

namespace TCGCards.Core.SpecialAbilities
{
    public class DamageStopper : TimedSpecialAbility
    {
        private Func<int, bool> action;

        public DamageStopper()
        {

        }

        public DamageStopper(Func<int, bool> action) : this(action, 2)
        {

        }

        public DamageStopper(Func<int, bool> action, int turnDuration) : base(turnDuration)
        {
            this.action = action;
        }

        public bool IsDamageIgnored(int damageDone) => action.Invoke(damageDone);
        public int Amount { get; set; }
    }
}

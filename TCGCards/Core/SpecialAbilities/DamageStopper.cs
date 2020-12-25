using System;

namespace TCGCards.Core.SpecialAbilities
{
    public class DamageStopper
    {
        private Func<int, bool> action;

        public DamageStopper()
        {

        }

        public DamageStopper(Func<int, bool> action) : this(action, Ability.UNTIL_YOUR_NEXT_TURN)
        {

        }

        public DamageStopper(Func<int, bool> action, int turnDuration)
        {
            this.action = action;
            TurnsLeft = turnDuration;
        }

        public bool IsDamageIgnored(int damageDone) => action.Invoke(damageDone);
        
        public int TurnsLeft { get; set; }
        public int Amount { get; set; }
        public bool LastsUntilDamageTaken { get; set; }
    }
}

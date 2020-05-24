using System;

namespace TCGCards.Core.SpecialAbilities
{
    public class AttackStopper : TimedSpecialAbility
    {
        private Func<PokemonCard, bool> action;

        public AttackStopper(Func<PokemonCard, bool> action) :this(action, 2)
        {

        }

        public AttackStopper(Func<PokemonCard, bool> action, int turnDuration) : base(turnDuration)
        {
            this.action = action;
        }

        public bool IsAttackIgnored(PokemonCard defender) => action.Invoke(defender);
    }
}

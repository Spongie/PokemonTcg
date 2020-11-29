using CardEditor.Views;
using System;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class DrainAttack : Attack
    {
        private float healingMultiplier = 1.0f;

        [DynamicInput("Healing modifier")]
        public float HealingMultiplier
        {
            get { return healingMultiplier; }
            set
            {
                healingMultiplier = value;
                FirePropertyChanged();
            }
        }

        public override void OnDamageDealt(int amount, Player owner)
        {
            var healing = (int)Math.Ceiling(amount * healingMultiplier);

            if (healing.ToString().Last() == '5')
            {
                healing += 5;
            }

            owner.ActivePokemonCard.DamageCounters -= healing;
        }
    }
}

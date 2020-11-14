using CardEditor.Views;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

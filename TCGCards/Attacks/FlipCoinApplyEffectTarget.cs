using CardEditor.Views;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCGCards.Attacks
{
    public class FlipCoinApplyEffectTarget : Attack
    {
        public FlipCoinApplyEffectTarget() : base()
        {
            Name = "Flip coin effect";
        }

        private StatusEffect statusEffect;

        [DynamicInput("Status Effect", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect StatusEffect
        {
            get { return statusEffect; }
            set 
            { 
                statusEffect = value;
                FirePropertyChanged();
            }
        }

    }
}

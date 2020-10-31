using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class HealEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private int amount;

        [DynamicInput("Heal amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Targeting type", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Heal";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            attachedTo.DamageCounters -= Amount;

            if (attachedTo.DamageCounters < 0)
            {
                attachedTo.DamageCounters = 0;
            }
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            PokemonCard target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent);

            target.DamageCounters -= Amount;
            
            if (target.DamageCounters < 0)
            {
                target.DamageCounters = 0;
            }
        }
    }
}

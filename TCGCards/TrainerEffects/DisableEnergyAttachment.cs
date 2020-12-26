using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DisableEnergyAttachment : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private bool coinFlip;

        public string EffectType
        {
            get
            {
                return "Disable Energy";
            }
        }

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coin flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }


        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            target.TemporaryAbilities.Add(new DisableEnergyAttachmentAbility(target));
        }
    }
}

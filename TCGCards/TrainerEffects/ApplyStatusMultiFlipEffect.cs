using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyStatusMultiFlipEffect : DataModel, IEffect
    {
        private int coinsToFlip = 1;
        private int headsForEffect = 1;
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private StatusEffect statusEffect = StatusEffect.Sleep;

        public string EffectType
        {
            get
            {
                return "Apply effect";
            }
        }



        [DynamicInput("Coins to flip")]
        public int CoinsToFlip
        {
            get { return coinsToFlip; }
            set
            {
                coinsToFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Heads required")]
        public int HeadsForEffect
        {
            get { return headsForEffect; }
            set
            {
                headsForEffect = value;
                FirePropertyChanged();
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

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            if (game.FlipCoins(CoinsToFlip) >= HeadsForEffect)
            {
                target.ApplyStatusEffect(StatusEffect, game);
            }
        }
    }
}

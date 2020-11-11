﻿using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.TrainerEffects
{
    public class ApplyDamagePreventionEffect : DataModel, IEffect
    {
        private int amount;
        private bool coinFlip;
        private int maxDamage;
        private bool onlyProtectSelf = true;

        [DynamicInput("Only protect self", InputControl.Boolean)]
        public bool OnlyProtectSelf
        {
            get { return onlyProtectSelf; }
            set
            {
                onlyProtectSelf = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Prevention Limit")]
        public int MaxDamage
        {
            get { return maxDamage; }
            set
            {
                maxDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coin flipped", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to prevent")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Apply Damage Prevention";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            throw new NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            DamageStopper damageStopper;

            if (maxDamage > 0)
            {
                damageStopper = new DamageStopper((x) => x <= maxDamage) { Amount = amount };
            }
            else
            {
                damageStopper = new DamageStopper((x) => true) { Amount = amount };
            }

            if (onlyProtectSelf)
            {
                caster.ActivePokemonCard.DamageStoppers.Add(damageStopper);
            }
            else
            {
                game.DamageStoppers.Add(damageStopper);
            }
        }
    }
}
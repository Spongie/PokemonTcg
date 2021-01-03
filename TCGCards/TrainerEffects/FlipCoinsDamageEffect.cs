using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class FlipCoinsDamageEffect : DataModel, IEffect
    {
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();
        private int amount;
        private TargetingMode targetingMode;
        private bool applyWeaknessResistance;
        private int coins;

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinflipConditional
        {
            get { return coinflipConditional; }
            set
            {
                coinflipConditional = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coins")]
        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target?", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Apply weakness resistance", InputControl.Boolean)]
        public bool ApplyWeaknessResistance
        {
            get { return applyWeaknessResistance; }
            set
            {
                applyWeaknessResistance = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Damage x Heads";
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
            if (!CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);
            var heads = game.FlipCoins(Coins);

            var damage = heads * Amount;

            if (applyWeaknessResistance)
            {
                damage = DamageCalculator.GetDamageAfterWeaknessAndResistance(damage, pokemonSource, target, null, null);
            }

            target.DealDamage(damage, game, pokemonSource, true);
        }
    }
}

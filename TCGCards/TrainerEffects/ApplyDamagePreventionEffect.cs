using CardEditor.Views;
using Entities.Models;
using System;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyDamagePreventionEffect : DataModel, IEffect
    {
        private int amount;
        private int maxDamage;
        private bool onlyProtectSelf = true;
        private bool lastsUntilDamage;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();

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

        [DynamicInput("Lasts until damage taken", InputControl.Boolean)]
        public bool LastsUntilDamage
        {
            get { return lastsUntilDamage; }
            set
            {
                lastsUntilDamage = value;
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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (!CoinflipConditional.IsOk(game, caster))
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

            damageStopper.LastsUntilDamageTaken = LastsUntilDamage;

            if (onlyProtectSelf)
            {
                pokemonSource.DamageStoppers.Add(damageStopper);
            }
            else
            {
                game.DamageStoppers.Add(damageStopper);
            }
        }
    }
}

using CardEditor.Views;
using Entities;
using System;
using System.Linq;

namespace TCGCards.Core.Abilities
{
    public class StopEverythingToMeAbility : Ability, IDamageTakenModifier, IStatusPreventer
    {
        private int damageToPrevent;
        private bool coinFlip;
        private bool useSamecoinFlip;
        private bool? lastFlip;
        private bool onlyStopWhenAsleep;
        private bool preventSleep;
        private bool preventParalyze;
        private bool preventConfuse;
        private bool preventBurn;
        private bool preventPoison;
        private bool onlyPreventAttacks;

        public StopEverythingToMeAbility():this(null)
        {

        }

        public StopEverythingToMeAbility(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.DamageTakenModifier;
        }

        [DynamicInput("Only disabled when asleep", InputControl.Boolean)]
        public bool OnlyStopWhenSleeping
        {
            get { return onlyStopWhenAsleep; }
            set
            {
                onlyStopWhenAsleep = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only prevent attacks", InputControl.Boolean)]
        public bool OnlyPreventAttacks
        {
            get { return onlyPreventAttacks; }
            set
            {
                onlyPreventAttacks = value;
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

        [DynamicInput("Use same coin for all things", InputControl.Boolean)]
        public bool UseSameCoinFlip
        {
            get { return useSamecoinFlip; }
            set
            {
                useSamecoinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to prevent")]
        public int DamageToPrevent
        {
            get { return damageToPrevent; }
            set
            {
                damageToPrevent = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Poison", InputControl.Boolean)]
        public bool PreventPoison
        {
            get { return preventPoison; }
            set
            {
                preventPoison = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Paralyze", InputControl.Boolean)]
        public bool PreventParalyze
        {
            get { return preventParalyze; }
            set
            {
                preventParalyze = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Sleep", InputControl.Boolean)]
        public bool PreventSleep
        {
            get { return preventSleep; }
            set
            {
                preventSleep = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Prevent Confuse", InputControl.Boolean)]
        public bool PreventConfuse
        {
            get { return preventConfuse; }
            set
            {
                preventConfuse = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Burn", InputControl.Boolean)]
        public bool PreventBurn
        {
            get { return preventBurn; }
            set
            {
                preventBurn = value;
                FirePropertyChanged();
            }
        }

        private bool IsCoinFlipHeads(GameField game)
        {
            if (!CoinFlip)
            {
                return true;
            }

            if (lastFlip.HasValue)
            {
                return lastFlip.Value;
            }

            lastFlip = game.FlipCoins(1) == 1;
            return lastFlip.Value;
        }

        public int GetModifiedDamage(int damageTaken, PokemonCard damageSource, GameField game)
        {
            if (OnlyPreventAttacks && game.GameState != GameFieldState.Attacking || !IsCoinFlipHeads(game))
            {
                return damageTaken;
            }

            return Math.Max(damageTaken - DamageToPrevent, 0);
        }

        public bool PreventsEffect(StatusEffect statusEffect, GameField game)
        {
            if (OnlyPreventAttacks && game.GameState != GameFieldState.Attacking || !IsCoinFlipHeads(game))
            {
                return false;
            }

            switch (statusEffect)
            {
                case StatusEffect.Sleep:
                    return preventSleep;
                case StatusEffect.Poison:
                    return preventPoison;
                case StatusEffect.Paralyze:
                    return preventParalyze;
                case StatusEffect.Burn:
                    return preventBurn;
                case StatusEffect.Confuse:
                    return preventConfuse;
                case StatusEffect.None:
                default:
                    return false;
            }
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            if (OnlyStopWhenSleeping)
            {
                return !PokemonOwner.AbilityDisabled
                    && !PokemonOwner.IsAsleep
                    && UsedTimes < Usages
                    && Effects.All(effect => effect.CanCast(game, caster, opponent));
            }

            return base.CanActivate(game, caster, opponent);
        }

        public override void EndTurn()
        {
            lastFlip = null;
            base.EndTurn();
        }
    }
}

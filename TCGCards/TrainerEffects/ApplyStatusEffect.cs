using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyStatusEffect : DataModel, IEffect
    {
        private StatusEffect statusEffect;
        private bool flipCoin;
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private StatusEffect secondaryEffect = StatusEffect.None;
        private bool useLastCoin;
        private bool checkTails;

        [DynamicInput("Use last coin flip?", InputControl.Boolean)]
        public bool UseLastCoin
        {
            get { return useLastCoin; }
            set
            {
                useLastCoin = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Trigger on tails instead?", InputControl.Boolean)]
        public bool CheckTails
        {
            get { return checkTails; }
            set
            {
                checkTails = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Flip Coin?", InputControl.Boolean)]
        public bool FlipCoin
        {
            get { return flipCoin; }
            set
            {
                flipCoin = value;
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

        [DynamicInput("Secondary Effect", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect SecondaryEffect
        {
            get { return secondaryEffect; }
            set
            {
                secondaryEffect = value;
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

        public string EffectType
        {
            get
            {
                return "Apply Status";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            ApplyEffectTo(attachedTo, StatusEffect, game);
            ApplyEffectTo(attachedTo, SecondaryEffect, game);
        }

        private void ApplyEffectTo(PokemonCard pokemon, StatusEffect effect, GameField game)
        {
            if (effect == StatusEffect.None)
            {
                return;
            }

            pokemon?.ApplyStatusEffect(effect, game);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var targetValue = CheckTails ? 0 : 1;
            var lastValue = game != null && game.LastCoinFlipResult ? 1 : 0;

            if (UseLastCoin && lastValue != targetValue)
            {
                return;
            }
            else if (flipCoin && game.FlipCoins(1) != targetValue)
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

            ApplyEffectTo(target, StatusEffect, game);
            ApplyEffectTo(target, SecondaryEffect, game);
        }
    }
}

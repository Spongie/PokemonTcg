using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class HealEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private int amount;
        private bool coinFlip;

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

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
            if (CoinFlip && CoinFlipper.FlipCoin())
            {
                return;
            }

            attachedTo.DamageCounters -= Amount;

            if (attachedTo.DamageCounters < 0)
            {
                attachedTo.DamageCounters = 0;
            }
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            PokemonCard target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

            target.DamageCounters -= Amount;
            
            if (target.DamageCounters < 0)
            {
                target.DamageCounters = 0;
            }
        }
    }
}

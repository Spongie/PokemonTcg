using System.Linq;
using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class FlipCoinAttack : Attack
    {
        private int coins;
        private int applyIfMoreThanThis = -1;
        private StatusEffect statusEffect;
        private TargetingMode targetingMode;
        private bool useLastDiscardCount;
        private bool discardEnergyForeachHeads;
        private EnergyTypes typeToDiscard;

        public FlipCoinAttack() : base()
        {
            Name = "Flip coin attack";
        }

        [DynamicInput("Number of coins")]
        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Apply Status if this or more Heads")]
        public int ApplyStatusIfThisOrMoreHeads
        {
            get { return applyIfMoreThanThis; }
            set
            {
                applyIfMoreThanThis = value;
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

        [DynamicInput("Use last discarded amount for #coins", InputControl.Boolean)]
        public bool UseLastDiscardCount
        {
            get { return useLastDiscardCount; }
            set
            {
                useLastDiscardCount = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int coins = UseLastDiscardCount ? game.LastDiscard : Coins;

            int heads = game.FlipCoins(coins);

            if (ApplyStatusIfThisOrMoreHeads != -1 &&  heads >= ApplyStatusIfThisOrMoreHeads)
            {
                var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, owner, opponent, owner.ActivePokemonCard);
                target?.ApplyStatusEffect(StatusEffect, game);
            }

            return heads * Damage;
        }
    }
}

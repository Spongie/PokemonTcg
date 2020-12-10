using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.TrainerEffects;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class FlipCoinAttack : Attack
    {
        private int coins;
        private int applyIfMoreThanThis = -1;
        private StatusEffect statusEffect;
        private TargetingMode targetingMode;

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

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int heads = game.FlipCoins(Coins);

            if (applyIfMoreThanThis != -1 && applyIfMoreThanThis >= heads)
            {
                var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, owner, opponent, owner.ActivePokemonCard);
                target?.ApplyStatusEffect(StatusEffect, game);
            }

            return heads * Damage;
        }
    }
}

using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackFailsOnTails : Attack
    {
        private bool isOneTime;
        private StatusEffect effect = StatusEffect.None;

        [DynamicInput("Applies status effect?", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect StatusEffect
        {
            get { return effect; }
            set
            {
                effect = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Fails forever?", InputControl.Boolean)]
        public bool IsOneTimeUse
        {
            get { return isOneTime; }
            set 
            { 
                isOneTime = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (game.FlipCoins(1) == 0)
            {
                if (isOneTime)
                {
                    foreverDisabled = true;
                }

                game?.GameLog.AddMessage("The attack did nothing");
                return 0;
            }

            if (StatusEffect != StatusEffect.None)
            {
                opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect, game);
            }

            return base.GetDamage(owner, opponent, game);
        }
    }
}

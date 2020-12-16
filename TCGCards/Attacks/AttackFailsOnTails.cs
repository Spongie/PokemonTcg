using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackFailsOnTails : Attack
    {
        private bool isOneTime;
        private StatusEffect effect = StatusEffect.None;
        private int selfDamage;

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

        [DynamicInput("Self damage on fail")]
        public int SelfDamage
        {
            get { return selfDamage; }
            set
            {
                selfDamage = value;
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

                if (SelfDamage > 0)
                {
                    owner.ActivePokemonCard.DealDamage(SelfDamage, game, owner.ActivePokemonCard, false);
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

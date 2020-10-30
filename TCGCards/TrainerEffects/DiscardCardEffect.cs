using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DiscardCardEffect : DataModel, IEffect
    {
        private int amount;
        private bool onlyOnCoinflip;

        [DynamicInput("Flip coin", InputControl.Boolean)]
        public bool OnlyOnCoinFlip
        {
            get { return onlyOnCoinflip; }
            set
            {
                onlyOnCoinflip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Cards to discard (-1 for all)")]
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
                return "Discard cards";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (amount == -1)
            {
                return true;
            }

            return caster.Hand.Count - 1 >= amount;
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            if (onlyOnCoinflip && game.FlipCoins(1) == 0)
            {
                return;
            }

            if (amount == -1)
            {
                caster.DiscardCards(caster.Hand.Select(card => card.Id).ToList());
                return;
            }

            GameUtils.DiscardCardsFromHand(caster, Amount);
        }
    }
}

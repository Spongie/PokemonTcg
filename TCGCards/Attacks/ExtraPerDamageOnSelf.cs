using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class ExtraPerDamageOnSelf : Attack
    {
        private int extraPerDamageCounter;
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra per damage counter")]
        public int ExtraPerDamageCounter
        {
            get { return extraPerDamageCounter; }
            set
            {
                extraPerDamageCounter = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (!CoinFlipConditional.IsOk(game, owner))
            {
                return 0;
            }

            var extraDamage = (owner.ActivePokemonCard.DamageCounters / 10) * extraPerDamageCounter;
            return Damage + extraDamage;
        }
    }
}

using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraPerDamageOnSelf : Attack
    {
        private int extraPerDamageCounter;

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
            var extraDamage = (owner.ActivePokemonCard.DamageCounters / 10) * extraPerDamageCounter;
            return Damage + extraDamage;
        }
    }
}

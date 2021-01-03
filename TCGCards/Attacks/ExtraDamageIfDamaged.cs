using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraDamageIfDamaged : Attack
    {
        private int extra;
        private bool ifNotDamaged;

        [DynamicInput("Extra damage")]
        public int Extra
        {
            get { return extra; }
            set
            {
                extra = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("If not damaged", InputControl.Boolean)]
        public bool IfNotDamaged
        {
            get { return ifNotDamaged; }
            set
            {
                ifNotDamaged = value;
                FirePropertyChanged();
            }
        }


        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (IfNotDamaged && opponent.ActivePokemonCard.DamageCounters == 0)
            {
                return Damage + Extra;
            }
            else if (!IfNotDamaged && opponent.ActivePokemonCard.DamageCounters > 0)
            {
                return Damage + Extra;
            }

            return Damage;
        }
    }
}

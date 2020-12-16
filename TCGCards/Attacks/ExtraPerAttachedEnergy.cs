using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraPerAttachedEnergy : Attack
    {
        private int extraPerEnergy;
        private bool countDefender;

        [DynamicInput("Extra per damage counter")]
        public int ExtraPerEnergy
        {
            get { return extraPerEnergy; }
            set
            {
                extraPerEnergy = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Count defender", InputControl.Boolean)]
        public bool CountDefender
        {
            get { return countDefender; }
            set
            {
                countDefender = value;
                FirePropertyChanged();
            }
        }


        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var target = CountDefender ? opponent.ActivePokemonCard : owner.ActivePokemonCard;

            var extraDamage = target.AttachedEnergy.Count * ExtraPerEnergy;
            return Damage + extraDamage;
        }
    }
}

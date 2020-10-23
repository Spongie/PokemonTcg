using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraPerAttachedEnergy : Attack
    {
        private int extraPerEnergy;

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

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var extraDamage = owner.ActivePokemonCard.AttachedEnergy.Count * ExtraPerEnergy;
            return Damage + extraDamage;
        }
    }
}

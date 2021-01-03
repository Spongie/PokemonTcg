using CardEditor.Views;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class OnlyIfMoreDamageAttack : Attack
    {
        private int cantUseBefore;

        [DynamicInput("Can't use before damage")]
        public int CantUseBeforeDamage
        {
            get { return cantUseBefore; }
            set
            {
                cantUseBefore = value;
                FirePropertyChanged();
            }
        }


        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            if (CantUseBeforeDamage < owner.ActivePokemonCard.DamageCounters)
            {
                return false;
            }

            return base.CanBeUsed(game, owner, opponent);
        }
    }
}

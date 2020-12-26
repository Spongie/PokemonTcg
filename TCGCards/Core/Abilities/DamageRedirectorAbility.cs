using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class DamageRedirectorAbility : Ability
    {
        private int amountToRedirect;
        private bool askYesNo;

        public DamageRedirectorAbility() :this(null)
        {

        }

        public DamageRedirectorAbility(PokemonCard owner) :base(owner)
        {

        }

        [DynamicInput("Amount to redirect")]
        public int AmountToRedirect
        {
            get { return amountToRedirect; }
            set
            {
                amountToRedirect = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Ask Yes/No?", InputControl.Boolean)]
        public bool AskYesNo
        {
            get { return askYesNo; }
            set
            {
                askYesNo = value;
                FirePropertyChanged();
            }
        }
    }
}

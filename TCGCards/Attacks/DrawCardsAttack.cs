using CardEditor.Views;

namespace TCGCards.Attacks
{
    public class DrawCardsAttack : Attack
    {
        private int amount;
        
        public DrawCardsAttack()
        {
            Name = "Draw cards";
        }

        [DynamicInput("Cards to draw")]
        public int Amount
        {
            get { return amount; }
            set 
            { 
                amount = value;
                FirePropertyChanged();
            }
        }

    }
}

using CardEditor.Views;
using Entities;
using TCGCards.Core;

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

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.DrawCards(Amount);
        }
    }
}

using CardEditor.Views;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class SelfDamageAttack : Attack
    {
        private int amount;

        public SelfDamageAttack() :base()
        {
            Name = "Self damage attack";
        }

        [DynamicInput("SelfDamage")]
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
            owner.ActivePokemonCard.DamageCounters += amount;
        }
    }
}

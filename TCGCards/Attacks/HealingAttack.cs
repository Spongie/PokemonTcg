using CardEditor.Views;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class HealingAttack : Attack
    {
        private int amount;

        [DynamicInput("Amount to heal")]
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
            owner.ActivePokemonCard.Heal(Amount, game);

            base.ProcessEffects(game, owner, opponent);
        }
    }
}

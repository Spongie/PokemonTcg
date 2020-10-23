using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackRequiresStatus : Attack
    {
        private StatusEffect statusEffect;

        [DynamicInput("Status Effect", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect StatusEffect
        {
            get { return statusEffect; }
            set
            {
                statusEffect = value;
                FirePropertyChanged();
            }
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            bool statusSatisfied = false;
            switch (StatusEffect)
            {
                case StatusEffect.Sleep:
                    statusSatisfied = opponent.ActivePokemonCard.IsAsleep;
                    break;
                case StatusEffect.Poison:
                    statusSatisfied = opponent.ActivePokemonCard.IsPoisoned;
                    break;
                case StatusEffect.Paralyze:
                    statusSatisfied = opponent.ActivePokemonCard.IsParalyzed;
                    break;
                case StatusEffect.Burn:
                    statusSatisfied = opponent.ActivePokemonCard.IsBurned;
                    break;
                case StatusEffect.Confuse:
                    statusSatisfied = opponent.ActivePokemonCard.IsConfused;
                    break;
                default:
                    break;
            }
            return statusSatisfied && base.CanBeUsed(game, owner, opponent);
        }
    }
}

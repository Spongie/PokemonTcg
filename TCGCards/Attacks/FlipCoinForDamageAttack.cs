using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinForDamageAttack : Attack
    {
        private bool damageOnSelf;

        [DynamicInput("Check damage on self", InputControl.Boolean)]
        public bool DamageOnSelf
        {
            get { return damageOnSelf; }
            set
            {
                damageOnSelf = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int coins = DamageOnSelf ? owner.ActivePokemonCard.DamageCounters / 10 : opponent.ActivePokemonCard.DamageCounters / 10;

            return game.FlipCoins(coins) * Damage;
        }
    }
}

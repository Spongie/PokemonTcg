using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class HalfHpAsDamage : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var damage = (opponent.ActivePokemonCard.Hp - opponent.ActivePokemonCard.DamageCounters) / 2;

            if (damage.ToString().EndsWith("5"))
                damage += 5;

            return damage;
        }
    }
}

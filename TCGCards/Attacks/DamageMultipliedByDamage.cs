using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class DamageMultipliedByDamage : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return Damage * (owner.ActivePokemonCard.DamageCounters / 10);
        }
    }
}

using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class MirrorMove : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return owner.ActivePokemonCard.DamageTakenLastTurn;
        }
    }
}

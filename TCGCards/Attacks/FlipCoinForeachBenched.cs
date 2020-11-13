using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinForeachBenched : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return game.FlipCoins(owner.BenchedPokemon.Count) * Damage;
        }
    }
}

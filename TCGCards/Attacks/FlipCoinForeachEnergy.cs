using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinForeachEnergy : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return game.FlipCoins(owner.ActivePokemonCard.AttachedEnergy.Count) * Damage;
        }
    }
}

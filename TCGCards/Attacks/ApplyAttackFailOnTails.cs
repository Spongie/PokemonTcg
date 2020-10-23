using Entities;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Attacks
{

    public class ApplyAttackFailOnTails : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.AttackStoppers.Add(new AttackStopper((x) =>
            {
                return CoinFlipper.FlipCoin() == CoinFlipper.TAILS;
            }));
        }
    }
}

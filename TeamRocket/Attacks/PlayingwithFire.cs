using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PlayingwithFire : Attack
    {
        public PlayingwithFire()
        {
            Name = "Flaying with Fire";
            Description = "Use this attack only if there are any [R] Energy cards attached to Dark Flareon. Flip a coin. If heads, discard 1 of those Energy cards and this attack does 30 damage plus 20 more damage. If tails, this attack does 30 damage.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
            {
                owner.ActivePokemonCard.DiscardEnergyCardOfType(EnergyTypes.Fire);
                return 50;
            }

            return 30;
        }
    }
}

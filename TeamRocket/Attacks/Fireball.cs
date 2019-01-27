using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Fireball : Attack
    {
        public Fireball()
        {
            Name = "Fireball";
            Description = "Use this attack only if there are any [R] Energy cards attached to Dark Charmeleon. Flip a coin. If heads, discard 1 of those Energy cards. If tails, this attack does nothing (not even damage).";
            DamageText = "70";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.TAILS)
            {
                return 0;
            }

            owner.ActivePokemonCard.DiscardEnergyCardOfType(EnergyTypes.Fire);

            return 70;
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return base.CanBeUsed(game, owner, opponent) && owner.ActivePokemonCard.AttachedEnergy.Any(e => e.EnergyType == EnergyTypes.Fire);
        }
    }
}

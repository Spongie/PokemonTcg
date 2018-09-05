using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TeamRocket.Attacks
{
    internal class LightningFlash : Attack
    {
        public LightningFlash()
        {
            Name = "Lightning Flash";
            Description = "If the Defending Pokémon tries to attack during your opponent's next turn, your opponent flips a coin. If tails, that attack does nothing.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            game.AttackStoppers.Add(new AttackStopper(() =>
            {
                return CoinFlipper.FlipCoin();
            }));
        }
    }
}

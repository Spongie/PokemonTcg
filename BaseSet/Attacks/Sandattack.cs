using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace BaseSet.Attacks
{
    internal class Sandattack : Attack
    {
        public Sandattack()
        {
            Name = "Sand-attack";
            Description = "If the Defending Pokémon tries to attack during your opponent's next turn, your opponent flips a coin. If tails, that attack does nothing.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            game.AttackStoppers.Add(new AttackStopper((defender) =>
            {
                return !CoinFlipper.FlipCoin();
            }));
        }
    }
}

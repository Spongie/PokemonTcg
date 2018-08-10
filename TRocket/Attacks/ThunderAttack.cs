using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class ThunderAttack : Attack
    {
        public ThunderAttack()
        {
            Name = "Thunder Attack";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed. If tails, Dark Jolteon does 10 damage to itself.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin())
            {
                opponent.ActivePokemonCard.IsParalyzed = true;
            }
            else
            {
                owner.ActivePokemonCard.DamageCounters += 10;
            }
        }
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class StunGas : Attack
    {
        public StunGas()
        {
            Name = "Stun Gas";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Poisoned. If tails, the Defending Pokémon is now Paralyzed.";
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin())
            {
                opponent.ActivePokemonCard.IsPoisoned = true;
            }
            else
            {
                opponent.ActivePokemonCard.IsParalyzed = true;
            }
        }
    }
}

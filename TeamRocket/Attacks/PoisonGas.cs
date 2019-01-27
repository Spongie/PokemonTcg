using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PoisonGasKoffing : Attack
    {
        public PoisonGasKoffing()
        {
            Name = "Poison Gas";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Poisoned.";
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1),
				new Energy(EnergyTypes.Colorless, 1)
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
        }
    }
}

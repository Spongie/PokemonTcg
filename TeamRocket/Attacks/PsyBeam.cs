using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PsyBeam : Attack
    {
        public PsyBeam()
        {
            Name = "Psybeam";
            Description = "Flip a coin. If heads, the defending pokemon is now confused";
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 3)
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
                opponent.ActivePokemonCard.IsConfused = true;
            }
        }
    }
}

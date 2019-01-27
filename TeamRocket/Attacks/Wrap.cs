using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Wrap : Attack
    {
        public Wrap()
        {
            Name = "Wrap";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed.";
            DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin())
            {
                opponent.ActivePokemonCard.IsParalyzed = true;
            }
        }
    }
}

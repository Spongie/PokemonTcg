using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    public class Dizziness : Attack
    {
        public Dizziness()
        {
            Name = "Dizziness";
            Description = "Draw a card";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.DrawCards(1);
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
    }

    public class PsyBeam : Attack
    {
        public PsyBeam()
        {
            Name = "Psybeam";
            Description = "Flip a coin. If heads, the defending pokemon is now confused";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 3)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
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

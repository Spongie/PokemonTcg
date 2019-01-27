using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Mischief : Attack
    {
        public Mischief()
        {
            Name = "Mischief";
            Description = "Shuffle your opponent's deck.";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.Deck.Shuffle();
        }
    }
}

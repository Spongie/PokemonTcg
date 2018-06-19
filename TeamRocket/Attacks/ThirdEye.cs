using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class ThirdEye : Attack
    {
        public ThirdEye()
        {
            Name = "Third Eye";
            Description = "Discard 1 Energy card attached to Dark Golduck in order to draw up to 3 cards.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}

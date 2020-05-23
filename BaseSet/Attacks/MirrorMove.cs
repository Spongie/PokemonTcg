using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class MirrorMove : Attack
    {
        public MirrorMove()
        {
            Name = "Mirror Move";
            Description = "If Pidgeotto was attacked last turn, do the final result of that attack on Pidgeotto to the Defending Pokémon.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}

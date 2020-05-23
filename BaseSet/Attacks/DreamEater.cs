using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class DreamEater : Attack
    {
        public DreamEater()
        {
            Name = "Dream Eater";
            Description = "You can't this attack unless the Defending Pokémon is Asleep.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		//TODO: Special effects
    }
}

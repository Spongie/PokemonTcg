using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class EnergyBomb : Attack
    {
        public EnergyBomb()
        {
            Name = "Energy Bomb";
            Description = "Take all Energy cards attached to Dark Electrode and attach them to your Benched Pokémon (in any way you choose). If you have no Benched Pokémon, discard all Energy cards attached to Dark Electrode.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}

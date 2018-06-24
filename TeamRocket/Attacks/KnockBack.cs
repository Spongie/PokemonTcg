using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class KnockBack : Attack
    {
        public KnockBack()
        {
            Name = "Knock Back";
            Description = "If your opponent has any Benched Pokémon, he or she chooses 1 of them and switches it with the Defending Pokémon. (Do the damage before switching the Pokémon.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}

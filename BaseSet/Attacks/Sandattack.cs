using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Sandattack : Attack
    {
        public Sandattack()
        {
            Name = "Sand-attack";
            Description = "If the Defending Pokémon tries to attack during your opponent's next turn, your opponent flips a coin. If tails, that attack does nothing.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}

using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Flail : Attack
    {
        public Flail()
        {
            Name = "Flail";
            Description = "Does 10 damage times number of damage counters on Magikarp.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 10 * (owner.ActivePokemonCard.DamageCounters / 10);
        }
    }
}

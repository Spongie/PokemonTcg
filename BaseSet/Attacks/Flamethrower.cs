using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Flamethrower : Attack
    {
        public Flamethrower()
        {
            Name = "Flamethrower";
            Description = "Discard 1 Energy card attached to Magmar in order to use this attack.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override void PayExtraCosts(GameField game, Player owner, Player opponent)
        {
            AttackUtils.DiscardAttachedEnergy(owner.ActivePokemonCard, 1);
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
    }
}

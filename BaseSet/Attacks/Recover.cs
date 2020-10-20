using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Recover : Attack
    {
        public Recover()
        {
            Name = "Recover";
            Description = "Discard 1 Energy card attached to Kadabra in order use this attack. Remove all damage counters from Kadabra.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override void PayExtraCosts(GameField game, Player owner, Player opponent)
        {
            AttackUtils.DiscardAttachedEnergy(owner.ActivePokemonCard, 1);
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.ActivePokemonCard.DamageCounters = 0;
        }
    }
}

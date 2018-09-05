using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class FinalBeam : Ability
    {
        public FinalBeam(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Dies;
        }

        public override void Activate(Player player, Player opponent, int damageTake)
        {
            if(Owner.IsAsleep || Owner.IsParalyzed || Owner.IsConfused)
                return;

            var damage = Owner.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Water);

            Owner.KnockedOutBy.DamageCounters += damage * 20;

            if(Owner.KnockedOutBy.IsDead())
                Owner.KnockedOutBy.KnockedOutBy = Owner;
        }

        public override void SetTarget(Card target)
        {
            
        }
    }
}

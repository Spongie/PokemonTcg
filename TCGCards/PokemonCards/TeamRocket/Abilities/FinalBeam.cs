using System.Linq;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Abilities
{
    public class FinalBeam : IAbility
    {
        public FinalBeam(IPokemonCard owner)
        {
            Owner = owner;
            TriggerType = TriggerType.Dies;
        }

        public void Activate(Player player, Player opponent)
        {
            if(Owner.IsAsleep || Owner.IsParalyzed || Owner.IsConfused)
                return;

            var damage = Owner.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Water);

            Owner.KnockedOutBy.DamageCounters += damage * 20;

            if(Owner.KnockedOutBy.IsDead())
                Owner.KnockedOutBy.KnockedOutBy = Owner;
        }

        public void SetTarget(IPokemonCard target)
        {
            
        }

        public TriggerType TriggerType { get; set; }

        public IPokemonCard Owner { get; }
    }
}

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

        protected override void Activate(Player player, Player opponent, int damageTake)
        {
            if(PokemonOwner.IsAsleep || PokemonOwner.IsParalyzed || PokemonOwner.IsConfused)
                return;

            var damage = PokemonOwner.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Water);

            PokemonOwner.KnockedOutBy.DamageCounters += damage * 20;

            if(PokemonOwner.KnockedOutBy.IsDead())
                PokemonOwner.KnockedOutBy.KnockedOutBy = PokemonOwner;
        }
    }
}

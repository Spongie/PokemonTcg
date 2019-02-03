using System.Collections.Generic;
using NetworkingCore;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class StickyGoo : CostModifierAbility
    {
        public StickyGoo(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.RetreatCost;
            ExtraCost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
            Name = "Sticky Goo";
            Description = "As long as Dark Muk is the active pokemon retreating costs 2 extra for your opponent";
        }

        public override bool IsActive() => PokemonOwner.Owner.ActivePokemonCard.Id.Equals(PokemonOwner.Id);

        public override HashSet<NetworkId> GetUnAffectedCards()
        {
            return new HashSet<NetworkId>(new[]
            {
                PokemonOwner.Id
            });
        }
    }
}

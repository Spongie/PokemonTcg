using System.Collections.Generic;
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
        }
    }
}

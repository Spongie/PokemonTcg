using System.Collections.Generic;

namespace TCGCards.Core
{
    public abstract class CostModifierAbility : PassiveAbility
    {
        protected CostModifierAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            
        }

        public List<Energy> ExtraCost { get; set; }
    }
}

using System.Collections.Generic;
using NetworkingCore;

namespace TCGCards.Core
{
    public abstract class CostModifierAbility : PassiveAbility
    {
        protected CostModifierAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            
        }

        public List<Energy> ExtraCost { get; set; }

        public virtual bool IsActive() => true;

        public virtual HashSet<NetworkId> GetUnAffectedCards() => new HashSet<NetworkId>();
    }
}

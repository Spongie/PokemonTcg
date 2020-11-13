using System.Collections.Generic;
using CardEditor.Views;
using NetworkingCore;

namespace TCGCards.Core.Abilities
{
    public class StopPokemonPowersAbility : PassiveAbility
    {
        private bool onlyWhenActive;

        public StopPokemonPowersAbility() : base(null)
        {
            ModifierType = PassiveModifierType.NoPokemonPowers;
        }

        public StopPokemonPowersAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.NoPokemonPowers;
        }

        [DynamicInput("Only when active", InputControl.Boolean)]
        public bool OnlyWhenActive
        {
            get { return onlyWhenActive; }
            set
            {
                onlyWhenActive = value;
                FirePropertyChanged();
            }
        }

        public virtual bool IsActive()
        {
            if (onlyWhenActive)
            {
                return PokemonOwner.Owner.ActivePokemonCard.Id.Equals(PokemonOwner.Id);
            }

            return true;
        }

        public virtual HashSet<NetworkId> GetUnAffectedCards()
        {
            return new HashSet<NetworkId>();
        }
    }
}

using System.Collections.Generic;
using CardEditor.Views;
using NetworkingCore;

namespace TCGCards.Core.Abilities
{
    public class StopAbilitiesAbility : PassiveAbility
    {
        private bool onlyWhenActive;

        public StopAbilitiesAbility() : this(null)
        {

        }

        public StopAbilitiesAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.StopAbilities;
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

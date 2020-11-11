using System.Collections.Generic;
using CardEditor.Views;
using NetworkingCore;

namespace TCGCards.Core
{
    public class StopTrainerCastsAbility : PassiveAbility
    {
        private bool onlyWhenActive;

        public StopTrainerCastsAbility() : base(null)
        {
            ModifierType = PassiveModifierType.StopTrainerCast;
        }

        public StopTrainerCastsAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.RetreatCost;
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

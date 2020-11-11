using System.Collections.Generic;
using System.Collections.ObjectModel;
using CardEditor.Views;
using Entities;
using NetworkingCore;

namespace TCGCards.Core
{
    public class CostModifierAbility : PassiveAbility
    {
        private ObservableCollection<Energy> extraCost;
        private bool onlyWhenActive;
        private bool worksOnSelf;

        public CostModifierAbility() :base(null)
        {
            ModifierType = PassiveModifierType.RetreatCost;
        }

        public CostModifierAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.RetreatCost;
        }

        [DynamicInput("Works on Self", InputControl.Boolean)]
        public bool WorksOnSelf
        {
            get { return worksOnSelf; }
            set
            {
                worksOnSelf = value;
                FirePropertyChanged();
            }
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

        [DynamicInput("Extra", InputControl.Grid, typeof(Energy))]
        public ObservableCollection<Energy> ExtraCost
        {
            get { return extraCost; }
            set
            {
                extraCost = value;
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
            var set = new HashSet<NetworkId>();

            if (!worksOnSelf)
            {
                set.Add(PokemonOwner.Id);
            }

            return set;
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CardEditor.Views;
using Entities;
using NetworkingCore;

namespace TCGCards.Core
{
    public class RetreatCostModifierAbility : PassiveAbility
    {
        private int amount;
        private bool onlyWhenActive;
        private bool worksOnSelf;

        public RetreatCostModifierAbility() :base(null)
        {
            ModifierType = PassiveModifierType.RetreatCost;
        }

        public RetreatCostModifierAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
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

        [DynamicInput("Amount Extra")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
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

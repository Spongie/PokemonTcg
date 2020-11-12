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
        private bool onlyWhenBenched;
        private bool onlyForYou;

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

        [DynamicInput("Only for you", InputControl.Boolean)]
        public bool OnlyForYou
        {
            get { return onlyForYou; }
            set
            {
                onlyForYou = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only when benched", InputControl.Boolean)]
        public bool OnlyWhenBenched
        {
            get { return onlyWhenBenched; }
            set
            {
                onlyWhenBenched = value;
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

        public virtual bool IsActive(GameField game)
        {
            if (onlyForYou && !game.ActivePlayer.Id.Equals(PokemonOwner.Owner.Id))
            {
                return false;
            }

            if (onlyWhenActive)
            {
                return PokemonOwner.Owner.ActivePokemonCard.Id.Equals(PokemonOwner.Id);
            }
            else if (onlyWhenBenched)
            {
                return PokemonOwner.Owner.BenchedPokemon.Contains(PokemonOwner);
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

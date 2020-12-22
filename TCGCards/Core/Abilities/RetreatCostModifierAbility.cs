using System.Collections.Generic;
using CardEditor.Views;
using NetworkingCore;

namespace TCGCards.Core.Abilities
{
    public class RetreatCostModifierAbility : PassiveAbility
    {
        private int amount;
        private bool worksOnSelf;
        private bool onlyForYou;

        public RetreatCostModifierAbility() : this(null)
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

            return base.CanActivate(game, PokemonOwner.Owner, game.GetOpponentOf(PokemonOwner.Owner));
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

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
        private string onlyWithLikeName;

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

        [DynamicInput("Only with name ")]
        public string OnlyWithNameLike
        {
            get { return onlyWithLikeName; }
            set
            {
                onlyWithLikeName = value;
                FirePropertyChanged();
            }
        }

        public virtual bool IsActive(GameField game)
        {
            if (!string.IsNullOrEmpty(OnlyWithNameLike) && !game.ActivePlayer.ActivePokemonCard.Name.ToLower().Contains(OnlyWithNameLike.ToLower()))
            {
                return false;
            }
            if (onlyForYou && !game.ActivePlayer.Id.Equals(PokemonOwner.Owner.Id))
            {
                return false;
            }

            var owner = GetActivator(game.ActivePlayer);
            return base.CanActivate(game, owner, game.GetOpponentOf(owner));
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

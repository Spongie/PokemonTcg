using CardEditor.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class ChangeTypeAbility : Ability
    {
        private bool onlyToColorsInGame;

        public ChangeTypeAbility() :this(null)
        {

        }

        public ChangeTypeAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
        }

        [DynamicInput("Only to colors already in game", InputControl.Boolean)]
        public bool OnlyToColorsInGame
        {
            get { return onlyToColorsInGame; }
            set
            {
                onlyToColorsInGame = value;
                FirePropertyChanged();
            }
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            var message = new SelectColorMessage("Select a new type for " + PokemonOwner.Name)
            {
                OnlyColorsInGame = OnlyToColorsInGame
            }.ToNetworkMessage(owner.Id);

            var response = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(message);

            PokemonOwner.Type = response.Color;
        }
    }
}

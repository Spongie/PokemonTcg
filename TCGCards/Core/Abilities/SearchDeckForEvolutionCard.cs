﻿using CardEditor.Views;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class SearchDeckForEvolutionCard : Ability
    {
        private int amount = 1;

        [DynamicInput("Cards to search")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }
        public SearchDeckForEvolutionCard(): this(null)
        {

        }
        public SearchDeckForEvolutionCard(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            var message = new DeckSearchMessage(owner.Deck.Cards.ToList(), new List<IDeckFilter> { new EvolutionPokemonFilter() }, amount).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            owner.DrawCardsFromDeck(response.Cards);
        }
    }
}

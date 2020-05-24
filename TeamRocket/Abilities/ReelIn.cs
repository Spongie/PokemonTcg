﻿using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class ReelIn : Ability
    {
        public ReelIn(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.EnterPlay;
            Name = "Reel In";
            Description = "When you play Dark Slowbro from your hand choose up to 3 pokemon cards from your discard pile and put them into your hand";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            var possibleChoices = owner.DiscardPile.OfType<PokemonCard>().Where(card => card.Stage == 0);

            if (!possibleChoices.Any())
            {
                return;
            }

            var message = new PickFromListMessage(possibleChoices, 0, 3).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var selectedCards = owner.DiscardPile.Where(card => response.Cards.Contains(card.Id)).ToList();
            owner.Hand.AddRange(selectedCards);
            foreach (var card in selectedCards)
            {
                owner.DiscardPile.Remove(card);
            }
        }
    }
}

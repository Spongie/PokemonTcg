using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class PokemonTrader : TrainerCard
    {
        public PokemonTrader(Player owner) : base(owner)
        {
            Name = "Pokémon Trader";
            Description = "Trade 1 of the Basic Pokémon or Evolution cards in your hand for 1 of the Basic Pokémon or Evolution cards from your deck. Show both cards to your opponent. Shuffle your deck afterward.";
        }

        public override bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.Hand.OfType<PokemonCard>().Any() && base.CanCast(game, caster, opponent);
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var availablePokemons = caster.Hand.OfType<PokemonCard>().ToList();

            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(availablePokemons, 1).ToNetworkMessage(game.Id));

            var cardToPutPack = caster.Hand.First(x => x.Id.Equals(response.Cards.First()));

            game.RevealCardsTo(response.Cards.ToList(), opponent);

            var availableCards = caster.Deck.Cards.OfType<PokemonCard>();
            var responseDeck = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(availableCards, 1).ToNetworkMessage(game.Id));

            caster.Deck.Cards.Push(cardToPutPack);

            var cardFromDeck = caster.Deck.Cards.First(x => x.Id.Equals(responseDeck.Cards.First()));

            game.RevealCardsTo(responseDeck.Cards.ToList(), opponent);

            caster.DrawCardsFromDeck(responseDeck.Cards);
        }
    }
}

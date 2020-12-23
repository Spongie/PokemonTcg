using Xunit;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    public class SearchDeckForCardEffectTests
    {
        [Fact]
        public void DeckSearched()
        {
            var effect = new SearchDeckForCardEffect()
            {
                CardType = CardType.Any
            };

            var player = new Player();
            player.Id = NetworkId.Generate();
            var target = new EnergyCard();
            player.Deck.Cards.Push(target);
            player.Deck.Cards.Push(new EnergyCard());
            player.Deck.Cards.Push(new EnergyCard());
            player.Deck.Cards.Push(new EnergyCard());
            player.Deck.Cards.Push(new EnergyCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { target.Id } });
            player.SetNetworkPlayer(sub);

            var game = new GameField();
            game.AddPlayer(player);
            effect.Process(game, player, null, null);

            Assert.Single(player.Hand);
            Assert.Equal(4, player.Deck.Cards.Count);
            Assert.Equal(target.Id, player.Hand[0].Id);
        }
    }
}

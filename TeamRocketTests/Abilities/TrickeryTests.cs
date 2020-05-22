using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class TrickeryTests
    {
        [TestMethod]
        public void Trigger_Test()
        {
            var player = new Player();
            player.PrizeCards.Add(new Oddish(player));
            player.Deck.Cards.Push(new DarkGloom(player));

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<TCGCards.Card>
            {
                player.PrizeCards.First()
            }));

            player.SetNetworkPlayer(networkPlayer);

            player.ActivePokemonCard = new Rattata(player);
            player.ActivePokemonCard.Ability.Trigger(player, null, 0, new GameLog());

            Assert.IsTrue(player.PrizeCards.First() is DarkGloom);
            Assert.IsTrue(player.Deck.Cards.First() is Oddish);
        }
    }
}

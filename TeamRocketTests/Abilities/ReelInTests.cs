using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class ReelInTests
    {
        [TestMethod]
        public void CardsAddedToHand()
        {
            var player = new Player();
            var pokemon = new DarkSlowbro(player);
            var ability = new ReelIn(pokemon);

            player.DiscardPile.Add(new Oddish(player));
            player.DiscardPile.Add(new DarkGloom(player));
            player.DiscardPile.Add(new DarkAlakazam(player));

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                player.DiscardPile[0].Id,
                player.DiscardPile[1].Id,
                player.DiscardPile[2].Id
            }));

            player.SetNetworkPlayer(networkPlayer);

            ability.Trigger(player, null, 0, new GameLog());

            Assert.AreEqual(3, player.Hand.Count);
            Assert.AreEqual(0, player.DiscardPile.Count);
        }
    }
}

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
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<Card>
            {
                player.DiscardPile[0],
                player.DiscardPile[1],
                player.DiscardPile[2]
            }));

            player.SetNetworkPlayer(networkPlayer);

            ability.Trigger(player, null, 0);

            Assert.AreEqual(3, player.Hand.Count);
            Assert.AreEqual(0, player.DiscardPile.Count);
        }
    }
}

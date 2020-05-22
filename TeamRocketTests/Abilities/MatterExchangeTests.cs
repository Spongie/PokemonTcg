using System.Collections.Generic;
using System.Linq;
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
    public class MatterExchangeTests
    {
        [TestMethod]
        public void Activate_Card_Discarded()
        {
            var player = new Player();
            var pokemon = new DarkSlowbro(player);
            var ability = new MatterExchange(pokemon);

            player.Hand.Add(new Oddish(player));
            player.Deck.Cards.Push(new DarkAlakazam(player));

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<Card>
            {
                player.Hand[0]
            }));

            player.SetNetworkPlayer(networkPlayer);

            ability.Trigger(player, null, 0, new GameLog());
            
            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.IsTrue(player.DiscardPile.First() is Oddish);
        }

        [TestMethod]
        public void Activate_Card_Drawn()
        {
            var player = new Player();
            var pokemon = new DarkSlowbro(player);
            var ability = new MatterExchange(pokemon);

            player.Hand.Add(new Oddish(player));
            player.Deck.Cards.Push(new DarkAlakazam(player));

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<Card>
            {
                player.Hand[0]
            }));

            player.SetNetworkPlayer(networkPlayer);

            ability.Trigger(player, null, 0, new GameLog());

            Assert.AreEqual(1, player.Hand.Count);
            Assert.IsTrue(player.Hand.First() is DarkAlakazam);
        }
    }
}

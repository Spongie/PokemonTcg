using BaseSet.TrainerCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;

namespace BaseSetTests.TrainerCards
{
    [TestClass]
    public class ComputerSearchTests
    {
        [TestMethod]
        public void CanUse_No_Other_Cards()
        {
            var player = new Player();
            var cs = new ComputerSearch();
            player.Hand.Add(cs);

            Assert.IsFalse(cs.CanCast(null, player, null));
        }

        [TestMethod]
        public void CanUse_1_Other_Cards()
        {
            var player = new Player();
            var cs = new ComputerSearch();
            player.Hand.Add(cs);
            player.Hand.Add(new WaterEnergy());

            Assert.IsFalse(cs.CanCast(null, player, null));
        }

        [TestMethod]
        public void CanUse_4_Other_Cards()
        {
            var player = new Player();
            var cs = new ComputerSearch();
            player.Hand.Add(cs);
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());

            Assert.IsTrue(cs.CanCast(null, player, null));
        }

        [TestMethod]
        public void Process()
        {
            var player = new Player();
            var cs = new ComputerSearch();
            var energy1 = new WaterEnergy();
            var energy2 = new WaterEnergy();

            var cardInDeck = new WaterEnergy();

            player.Deck.Cards.Push(cardInDeck);

            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(energy1);
            player.Hand.Add(energy2);

            INetworkPlayer networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(Arg.Is<NetworkMessage>(x => x.MessageType == MessageTypes.PickFromList))
                .Returns(new CardListMessage(new List<NetworkId> { energy1.Id, energy2.Id }));

            networkPlayer.SendAndWaitForResponse<CardListMessage>(Arg.Is<NetworkMessage>(x => x.MessageType == MessageTypes.DeckSearch))
                .Returns(new CardListMessage(new List<NetworkId> { cardInDeck.Id }));

            player.SetNetworkPlayer(networkPlayer);

            cs.Process(null, player, null);

            Assert.AreEqual(2, player.DiscardPile.Count);
            Assert.AreEqual(3, player.Hand.Count);
            Assert.AreEqual(0, player.Deck.Cards.Count);
            Assert.IsNotNull(player.Hand.FirstOrDefault(card => card.Id.Equals(cardInDeck.Id)));
        }
    }
}

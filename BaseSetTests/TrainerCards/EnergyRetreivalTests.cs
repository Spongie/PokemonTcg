using BaseSet.TrainerCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;

namespace BaseSetTests.TrainerCards
{
    [TestClass]
    public class EnergyRetreivalTests
    {
        [TestMethod]
        public void NoOtherCardInHand()
        {
            var player = new Player();
            var energyRetreival = new EnergyRetrieval();
            player.Hand.Add(energyRetreival);

            Assert.IsFalse(energyRetreival.CanCast(null, player, null));
        }

        [TestMethod]
        public void TwoOtherCardInHand()
        {
            var player = new Player();
            var energyRetreival = new EnergyRetrieval();
            player.Hand.Add(energyRetreival);
            player.Hand.Add(new FightingEnergy());
            player.Hand.Add(new FightingEnergy());

            Assert.IsTrue(energyRetreival.CanCast(null, player, null));
        }

        [TestMethod]
        public void Process()
        {
            var player = new Player();
            var energyRetreival = new EnergyRetrieval();
            var discardTarget = new WaterEnergy();

            player.Hand.Add(energyRetreival);
            player.Hand.Add(discardTarget);
            player.Hand.Add(new FightingEnergy());
            player.Hand.Add(new FightingEnergy());

            var grassEnergy = new GrassEnergy();
            var fireEnergy = new FireEnergy();

            player.DiscardPile.Add(grassEnergy);
            player.DiscardPile.Add(fireEnergy);

            INetworkPlayer fakeNetwork = Substitute.For<INetworkPlayer>();
            fakeNetwork.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>())
                .ReturnsForAnyArgs(new CardListMessage(new List<NetworkId> { discardTarget.Id }), new CardListMessage(new List<NetworkId> { grassEnergy.Id, fireEnergy.Id }));

            player.SetNetworkPlayer(fakeNetwork);

            energyRetreival.Process(new GameField(), player, null);

            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.AreEqual(5, player.Hand.Count);

            Assert.IsTrue(player.DiscardPile.Contains(discardTarget));

            Assert.IsTrue(player.Hand.Contains(fireEnergy));
            Assert.IsTrue(player.Hand.Contains(grassEnergy));
        }
    }
}

using BaseSet.TrainerCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace BaseSetTests.TrainerCards
{
    [TestClass]
    public class BillTests
    {
        [TestMethod]
        public void Process()
        {
            var player = new Player();
            var bill = new Bill();

            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());

            bill.Process(null, player, null);

            Assert.AreEqual(2, player.Hand.Count);
            Assert.AreEqual(3, player.Deck.Cards.Count);
        }
    }
}

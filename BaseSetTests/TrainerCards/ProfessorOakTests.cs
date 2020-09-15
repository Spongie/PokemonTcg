using BaseSet.TrainerCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace BaseSetTests.TrainerCards
{
    [TestClass]
    public class ProfessorOakTests
    {
        [TestMethod]
        public void Process_New_Hand()
        {
            var player = new Player();
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());

            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());

            new ProfessorOak().Process(null, player, null);

            Assert.AreEqual(4, player.DiscardPile.Count);
            Assert.AreEqual(7, player.Hand.Count);
        }

        [TestMethod]
        public void Process_Not_Discarding_Self()
        {
            var player = new Player();
            var oak = new ProfessorOak();

            player.Hand.Add(oak);
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());
            player.Hand.Add(new WaterEnergy());

            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());
            player.Deck.Cards.Push(new WaterEnergy());

            oak.Process(null, player, null);

            Assert.AreEqual(4, player.DiscardPile.Count);
        }
    }
}

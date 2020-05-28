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
        public void Process()
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
        }
    }
}

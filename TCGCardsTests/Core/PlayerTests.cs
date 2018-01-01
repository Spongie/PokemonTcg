using TCGCards.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.EnergyCards;
using TCGCards.PokemonCards.TeamRocket;

namespace TCGCards.Core.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void SetActiePokemon_CardInHand()
        {
            var p = new Player();
            var card = new Magikarp(p);
            p.Hand.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.Hand.Contains(card));
        }

        [TestMethod()]
        public void SetActiePokemon_CardOnBench()
        {
            var p = new Player();
            var card = new Magikarp(p);
            p.BenchedPokemon.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.BenchedPokemon.Contains(card));
        }

        [TestMethod()]
        public void SetActiePokemon_SpotTaken()
        {
            var p = new Player();
            var card = new Magikarp(p);
            var card2 = new Magikarp(p);

            p.SetActivePokemon(card);
            p.SetActivePokemon(card2);

            Assert.AreEqual(card.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_NoEnergy()
        {
            var p = new Player();
            var card = new Magikarp(p);
            var card2 = new Magikarp(p);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.RetreatActivePokemon(p.BenchedPokemon.First());

            Assert.AreEqual(card.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_Energy()
        {
            var p = new Player();
            var card = new Magikarp(p);
            var card2 = new Magikarp(p);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.AttachEnergyToPokemon(new WaterEnergy(), p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.First());

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_StatusReset()
        {
            var p = new Player();
            var card = new Magikarp(p)
            {
                IsBurned = true
            };

            var card2 = new Magikarp(p);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.AttachEnergyToPokemon(new WaterEnergy(), p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.First());

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(card.IsBurned);
        }

        [TestMethod()]
        public void PlayCardTest_NoActive()
        {
            var p = new Player();
            var card = new Magikarp(p);

            p.PlayCard(card);

            Assert.AreEqual(card, p.ActivePokemonCard);
        }

        [TestMethod()]
        public void PlayCardTest_Active()
        {
            var p = new Player();
            var card = new Magikarp(p);
            var card2 = new Magikarp(p);
            p.PlayCard(card);
            p.PlayCard(card2);

            Assert.AreEqual(card, p.ActivePokemonCard);
            Assert.AreEqual(card2, p.BenchedPokemon.First());
        }
    }
}
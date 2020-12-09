using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void SetActivePokemon_CardInHand()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            p.Hand.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.Hand.Contains(card));
        }

        [TestMethod()]
        public void SetActivePokemon_CardOnBench()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            p.BenchedPokemon.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.BenchedPokemon.Contains(card));
        }

        [TestMethod()]
        public void SetActivePokemon_SpotTaken()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            var card2 = new TestPokemonCard(p);

            p.SetActivePokemon(card);
            p.SetActivePokemon(card2);

            Assert.AreEqual(card.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(p.BenchedPokemon.Any());
        }

        [TestMethod]
        public void RetreatActivePokemon_NoEnergy()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            var card2 = new TestPokemonCard(p);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.RetreatActivePokemon(p.BenchedPokemon.First(), new List<EnergyCard>(), null);

            Assert.AreEqual(card.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_Energy()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            var card2 = new TestPokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard, null);

            p.RetreatActivePokemon(p.BenchedPokemon.First(), new List<EnergyCard>(), null);

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_Energy_Removed()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            var card2 = new TestPokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.First(), new List<EnergyCard>(p.ActivePokemonCard.AttachedEnergy), null);

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(card.AttachedEnergy.Any());
        }

        [TestMethod]
        public void RetreatActivePokemon_StatusReset()
        {
            var p = new Player();
            var card = new TestPokemonCard(p)
            {
                IsBurned = true
            };

            var card2 = new TestPokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.First(), new List<EnergyCard>(p.ActivePokemonCard.AttachedEnergy), null);

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(card.IsBurned);
        }

        [TestMethod()]
        public void PlayCardTest_NoActive()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);

            p.PlayCard(card);

            Assert.AreEqual(card, p.ActivePokemonCard);
        }

        [TestMethod()]
        public void PlayCardTest_Active()
        {
            var p = new Player();
            var card = new TestPokemonCard(p);
            var card2 = new TestPokemonCard(p);
            p.PlayCard(card);
            p.PlayCard(card2);

            Assert.AreEqual(card, p.ActivePokemonCard);
            Assert.AreEqual(card2, p.BenchedPokemon.First());
        }

        [TestMethod]
        public void KillActivePokemon_Basic_NothingAttached()
        {
            var player = new Player();
            player.ActivePokemonCard = new TestPokemonCard(player);

            player.KillActivePokemon();

            Assert.IsNull(player.ActivePokemonCard);
            Assert.AreEqual(1, player.DiscardPile.Count);
        }

        [TestMethod]
        public void KillActivePokemon_Basic_AttachedEnergy()
        {
            var player = new Player();
            player.ActivePokemonCard = new TestPokemonCard(player);
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());

            player.KillActivePokemon();

            Assert.IsNull(player.ActivePokemonCard);
            Assert.AreEqual(3, player.DiscardPile.Count);
        }

        [TestMethod]
        public void KillActivePokemon_StageOne_AttachedEnergy()
        {
            var player = new Player();
            var abra = new TestPokemonCard(player);
            var kadabra = new TestPokemonCard(player);

            abra.Evolve(kadabra);

            player.ActivePokemonCard = kadabra;
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());

            player.KillActivePokemon();

            Assert.IsNull(player.ActivePokemonCard);
            Assert.AreEqual(4, player.DiscardPile.Count);
        }

        [TestMethod]
        public void KillActivePokemon_StageOne_NoEnergy()
        {
            var player = new Player();
            var abra = new TestPokemonCard(player);
            var kadabra = new TestPokemonCard(player);

            abra.Evolve(kadabra);

            player.ActivePokemonCard = kadabra;

            player.KillActivePokemon();

            Assert.IsNull(player.ActivePokemonCard);
            Assert.AreEqual(2, player.DiscardPile.Count);
        }
    }
}
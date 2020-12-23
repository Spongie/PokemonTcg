using TCGCards.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace TCGCards.Core.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void SetActivePokemon_CardInHand()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            p.Hand.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.Hand.Contains(card));
        }

        [TestMethod()]
        public void SetActivePokemon_CardOnBench()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            p.BenchedPokemon.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.BenchedPokemon.Contains(card));
        }

        [TestMethod()]
        public void SetActivePokemon_SpotTaken()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);

            p.SetActivePokemon(card);
            p.SetActivePokemon(card2);

            Assert.AreEqual(card.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(p.BenchedPokemon.Count > 0);
        }

        [TestMethod]
        public void RetreatActivePokemon_NoEnergy()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(), null);

            Assert.AreEqual(card.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_Energy()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard, null);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(), null);

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
        }

        [TestMethod]
        public void RetreatActivePokemon_Energy_Removed()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(p.ActivePokemonCard.AttachedEnergy), null);

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(card.AttachedEnergy.Any());
        }

        [TestMethod]
        public void RetreatActivePokemon_StatusReset()
        {
            var p = new Player();
            var card = new PokemonCard(p)
            {
                IsBurned = true
            };

            var card2 = new PokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(p.ActivePokemonCard.AttachedEnergy), null);

            Assert.AreEqual(card2.Id, p.ActivePokemonCard.Id);
            Assert.IsFalse(card.IsBurned);
        }

        [TestMethod()]
        public void PlayCardTest_NoActive()
        {
            var p = new Player();
            var card = new PokemonCard(p);

            p.PlayCard(card);

            Assert.AreEqual(card, p.ActivePokemonCard);
        }

        [TestMethod()]
        public void PlayCardTest_Active()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);
            p.PlayCard(card);
            p.PlayCard(card2);

            Assert.AreEqual(card, p.ActivePokemonCard);
            Assert.AreEqual(card2, p.BenchedPokemon.ValidPokemonCards.First());
        }

        [TestMethod]
        public void KillActivePokemon_Basic_NothingAttached()
        {
            var player = new Player();
            player.ActivePokemonCard = new PokemonCard(player);

            player.KillActivePokemon();

            Assert.IsNull(player.ActivePokemonCard);
            Assert.AreEqual(1, player.DiscardPile.Count);
        }

        [TestMethod]
        public void KillActivePokemon_Basic_AttachedEnergy()
        {
            var player = new Player();
            player.ActivePokemonCard = new PokemonCard(player);
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
            var abra = new PokemonCard(player);
            var kadabra = new PokemonCard(player);

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
            var abra = new PokemonCard(player);
            var kadabra = new PokemonCard(player);

            abra.Evolve(kadabra);

            player.ActivePokemonCard = kadabra;

            player.KillActivePokemon();

            Assert.IsNull(player.ActivePokemonCard);
            Assert.AreEqual(2, player.DiscardPile.Count);
        }

        [TestMethod]
        public void KillActivePokemonTest_FullyHealedInDiscard()
        {
            var player = new Player();
            var pokemon = new PokemonCard()
            {
                Owner = player,
                Hp = 50
            };

            player.ActivePokemonCard = pokemon;

            pokemon.DamageCounters = 60;
            pokemon.ApplyStatusEffect(StatusEffect.Burn, null);
            pokemon.ApplyStatusEffect(StatusEffect.Confuse, null);
            pokemon.ApplyStatusEffect(StatusEffect.Paralyze, null);
            pokemon.ApplyStatusEffect(StatusEffect.Poison, null);
            pokemon.ApplyStatusEffect(StatusEffect.Sleep, null);

            player.KillActivePokemon();

            Assert.AreEqual(1, player.DiscardPile.Count);

            Assert.IsFalse(pokemon.IsAsleep);
            Assert.IsFalse(pokemon.IsPoisoned);
            Assert.IsFalse(pokemon.IsParalyzed);
            Assert.IsFalse(pokemon.IsBurned);
            Assert.IsFalse(pokemon.IsConfused);
            Assert.AreEqual(0, pokemon.DamageCounters);
        }

        [TestMethod()]
        public void KillBenchedPokemonTest()
        {
            var player = new Player();
            var pokemon = new PokemonCard()
            {
                Owner = player,
                Hp = 50
            };

            player.ActivePokemonCard = new PokemonCard();
            player.BenchedPokemon.Add(pokemon);

            pokemon.DamageCounters = 60;
            pokemon.ApplyStatusEffect(StatusEffect.Burn, null);
            pokemon.ApplyStatusEffect(StatusEffect.Confuse, null);
            pokemon.ApplyStatusEffect(StatusEffect.Paralyze, null);
            pokemon.ApplyStatusEffect(StatusEffect.Poison, null);
            pokemon.ApplyStatusEffect(StatusEffect.Sleep, null);

            player.KillBenchedPokemon(pokemon);

            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.AreEqual(0, player.BenchedPokemon.Count);

            Assert.IsFalse(pokemon.IsAsleep);
            Assert.IsFalse(pokemon.IsPoisoned);
            Assert.IsFalse(pokemon.IsParalyzed);
            Assert.IsFalse(pokemon.IsBurned);
            Assert.IsFalse(pokemon.IsConfused);
            Assert.AreEqual(0, pokemon.DamageCounters);
        }
    }
}
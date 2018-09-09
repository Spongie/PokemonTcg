using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards;
using TCGCards.Core;
using TeamRocket.EnergyCards;

namespace TeamRocketTests.EnergyCards
{
    [TestClass]
    public class PotionEnergyTests
    {
        [TestMethod]
        public void OnAttached_Pokemon_Hand_Healer()
        {
            var player = new Player();
            var energy = new PotionEnergy();
            player.Hand.Add(energy);
            var pokemon = new PokemonCard(player);
            pokemon.DamageCounters += 20;

            player.AttachEnergyToPokemon(energy, pokemon);

            Assert.AreEqual(10, pokemon.DamageCounters);
        }

        [TestMethod]
        public void OnAttached_Pokemon_NotHand_Not_Healed()
        {
            var player = new Player();
            var pokemon = new PokemonCard(player);
            pokemon.DamageCounters += 20;
            player.AttachEnergyToPokemon(new PotionEnergy(), pokemon);

            Assert.AreEqual(20, pokemon.DamageCounters);
        }
    }
}

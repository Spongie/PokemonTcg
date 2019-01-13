using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;
using TeamRocket.EnergyCards;

namespace TeamRocketTests.EnergyCards
{
    [TestClass]
    public class FullHealEnergyTests
    {
        [TestMethod]
        public void OnAttached_Pokemon_Hand_Cleared_Status()
        {
            var player = new Player();
            var energy = new FullHealEnergy();
            player.Hand.Add(energy);
            var pokemon = new PokemonCard(player);
            pokemon.IsAsleep = true;
            pokemon.IsConfused = true;
            pokemon.IsParalyzed = true;
            pokemon.IsPoisoned = true;

            player.AttachEnergyToPokemon(energy, pokemon);

            Assert.IsFalse(pokemon.IsConfused);
            Assert.IsFalse(pokemon.IsAsleep);
            Assert.IsFalse(pokemon.IsPoisoned);
            Assert.IsFalse(pokemon.IsParalyzed);
        }

        [TestMethod]
        public void OnAttached_Pokemon_NotHand_Status_Not_Cleared()
        {
            var player = new Player();
            var pokemon = new PokemonCard(player);
            pokemon.IsAsleep = true;
            pokemon.IsConfused = true;
            pokemon.IsParalyzed = true;
            pokemon.IsPoisoned = true;

            player.AttachEnergyToPokemon(new FullHealEnergy(), pokemon);

            Assert.IsTrue(pokemon.IsConfused);
            Assert.IsTrue(pokemon.IsAsleep);
            Assert.IsTrue(pokemon.IsPoisoned);
            Assert.IsTrue(pokemon.IsParalyzed);
        }
    }
}

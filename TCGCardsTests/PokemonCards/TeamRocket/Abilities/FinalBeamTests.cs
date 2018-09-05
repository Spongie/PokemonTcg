using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace TCGCards.PokemonCards.TeamRocket.Abilities.Tests
{
    [TestClass()]
    public class FinalBeamTests
    {
        [TestMethod()]
        public void Activate_IsAsleep()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();

            activePokemon.IsAsleep = true;
            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Activate(null, null, 0);

            Assert.AreEqual(0, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_IsConfused()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();

            activePokemon.IsConfused = true;
            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Activate(null, null, 0);

            Assert.AreEqual(0, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_IsParalyzed()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();

            activePokemon.IsParalyzed = true;
            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Activate(null, null, 0);

            Assert.AreEqual(0, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_1_Energy()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();

            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Activate(null, null, 0);

            Assert.AreEqual(20, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_2_Energy()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();

            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Activate(null, null, 0);

            Assert.AreEqual(40, otherPokemon.DamageCounters);
        }
    }
}
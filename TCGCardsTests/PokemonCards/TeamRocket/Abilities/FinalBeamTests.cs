using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
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

            activePokemon.Ability.Trigger(null, null, 0, new GameLog());

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

            activePokemon.Ability.Trigger(null, null, 0, new GameLog());

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

            activePokemon.Ability.Trigger(null, null, 0, new GameLog());

            Assert.AreEqual(0, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_1_Energy()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Trigger(null, null, 0, new GameLog());

            Assert.AreEqual(20, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_2_Energy()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Trigger(null, null, 0, new GameLog());

            Assert.AreEqual(40, otherPokemon.DamageCounters);
        }

        [TestMethod()]
        public void Activate_Tails_Nothing()
        {
            var activePokemon = new DarkGyarados();
            var otherPokemon = new Magikarp();
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.AttachedEnergy.Add(new WaterEnergy());
            activePokemon.KnockedOutBy = otherPokemon;

            activePokemon.Ability.Trigger(null, null, 0, new GameLog());

            Assert.AreEqual(0, otherPokemon.DamageCounters);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class ExtraPerDamageOnTargetTests
    {
        [TestMethod()]
        public void GetDamageTest_No_Damage()
        {
            var attack = new ExtraPerDamageOnTarget()
            {
                Damage = 10,
                ExtraPerDamageCounter = 10
            };

            var target = new PokemonCard();

            Assert.AreEqual(10, attack.GetDamage(new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }, null).NormalDamage);
        }

        [TestMethod()]
        public void GetDamageTest_20_Damage()
        {
            var attack = new ExtraPerDamageOnTarget()
            {
                Damage = 10,
                ExtraPerDamageCounter = 10
            };

            var target = new PokemonCard() { DamageCounters = 20 };

            Assert.AreEqual(30, attack.GetDamage(new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }, null).NormalDamage);
        }
    }
}
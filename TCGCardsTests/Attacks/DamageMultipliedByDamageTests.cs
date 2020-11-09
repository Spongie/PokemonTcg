using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class DamageMultipliedByDamageTests
    {
        [TestMethod()]
        public void GetDamageTest_No_Damage()
        {
            var attack = new DamageMultipliedByDamage()
            {
                Damage = 10
            };

            var target = new PokemonCard();

            Assert.AreEqual(0, attack.GetDamage(new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }, null).NormalDamage);
        }

        [TestMethod()]
        public void GetDamageTest_20_Damage()
        {
            var attack = new DamageMultipliedByDamage()
            {
                Damage = 10,
            };

            var target = new PokemonCard() { DamageCounters = 20 };

            Assert.AreEqual(20, attack.GetDamage(new Player { ActivePokemonCard = target }, new Player { ActivePokemonCard = new PokemonCard() }, null).NormalDamage);
        }
    }
}
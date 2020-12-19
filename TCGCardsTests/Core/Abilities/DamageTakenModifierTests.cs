using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TCGCards.Core.Abilities.Tests
{
    [TestClass]
    public class DamageTakenModifierTests
    {
        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Down_10()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.AreEqual(0, ability.GetModifiedDamage(10, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Down_20()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.AreEqual(10, ability.GetModifiedDamage(20, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Down_30()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.AreEqual(10, ability.GetModifiedDamage(30, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Down_40()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.AreEqual(20, ability.GetModifiedDamage(40, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Up_10()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.AreEqual(10, ability.GetModifiedDamage(10, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Up_20()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.AreEqual(10, ability.GetModifiedDamage(20, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Up_30()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.AreEqual(20, ability.GetModifiedDamage(30, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_Half_Round_Up_40()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.AreEqual(20, ability.GetModifiedDamage(40, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_20_Round_Down_10()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 20f,
                RoundDown = true
            };


            Assert.AreEqual(0, ability.GetModifiedDamage(10, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_20_Round_Down_20()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 20f,
                RoundDown = true
            };


            Assert.AreEqual(0, ability.GetModifiedDamage(20, null));
        }

        [TestMethod]
        public void GetModifiedDamageTest_20_Round_Down_30()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 20f,
                RoundDown = true
            };


            Assert.AreEqual(10, ability.GetModifiedDamage(30, null));
        }
    }
}
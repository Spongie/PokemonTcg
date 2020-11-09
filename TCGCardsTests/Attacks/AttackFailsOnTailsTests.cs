using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class AttackFailsOnTailsTests
    {
        [TestMethod()]
        public void GetDamage_Heads()
        {
            var attack = new AttackFailsOnTails() { Damage = 10 };

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            Assert.AreEqual(10, attack.GetDamage(null, null, new GameField()).NormalDamage);
        }

        [TestMethod()]
        public void GetDamage_Tails()
        {
            var attack = new AttackFailsOnTails() { Damage = 10 };

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            Assert.AreEqual(0, attack.GetDamage(null, null, new GameField()).NormalDamage);
        }

        [TestMethod()]
        public void GetDamage_Forever()
        {
            var attack = new AttackFailsOnTails() { Damage = 10 };

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            Assert.AreEqual(0, attack.GetDamage(null, null, new GameField()).NormalDamage);
            Assert.IsFalse(attack.CanBeUsed(new GameField(), new Player(), new Player()));
        }
    }
}
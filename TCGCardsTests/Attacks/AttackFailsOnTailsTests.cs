using Xunit;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class AttackFailsOnTailsTests
    {
        [Fact]
        public void GetDamage_Heads()
        {
            var attack = new AttackFailsOnTails() { Damage = 10 };

            Assert.Equal(10, attack.GetDamage(null, null, new GameField().WithFlips(CoinFlipper.HEADS)).NormalDamage);
        }

        [Fact]
        public void GetDamage_Tails()
        {
            var attack = new AttackFailsOnTails() { Damage = 10 };

            Assert.Equal(0, attack.GetDamage(null, null, new GameField().WithFlips(CoinFlipper.TAILS)).NormalDamage);
        }

        [Fact]
        public void GetDamage_Forever()
        {
            var attack = new AttackFailsOnTails() { Damage = 10 };

            Assert.Equal(0, attack.GetDamage(null, null, new GameField().WithFlips(CoinFlipper.TAILS)).NormalDamage);
            Assert.False(attack.CanBeUsed(new GameField(), new Player(), new Player()));
        }
    }
}
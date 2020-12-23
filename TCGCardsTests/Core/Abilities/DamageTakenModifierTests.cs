using Xunit;

namespace TCGCards.Core.Abilities.Tests
{
    public class DamageTakenModifierTests
    {
        [Fact]
        public void GetModifiedDamageTest_Half_Round_Down_10()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.Equal(0, ability.GetModifiedDamage(10, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Down_20()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.Equal(10, ability.GetModifiedDamage(20, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Down_30()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.Equal(10, ability.GetModifiedDamage(30, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Down_40()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = true
            };


            Assert.Equal(20, ability.GetModifiedDamage(40, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Up_10()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.Equal(10, ability.GetModifiedDamage(10, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Up_20()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.Equal(10, ability.GetModifiedDamage(20, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Up_30()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.Equal(20, ability.GetModifiedDamage(30, null));
        }

        [Fact]
        public void GetModifiedDamageTest_Half_Round_Up_40()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 0.5f,
                RoundDown = false
            };


            Assert.Equal(20, ability.GetModifiedDamage(40, null));
        }

        [Fact]
        public void GetModifiedDamageTest_20_Round_Down_10()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 20f,
                RoundDown = true
            };


            Assert.Equal(0, ability.GetModifiedDamage(10, null));
        }

        [Fact]
        public void GetModifiedDamageTest_20_Round_Down_20()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 20f,
                RoundDown = true
            };


            Assert.Equal(0, ability.GetModifiedDamage(20, null));
        }

        [Fact]
        public void GetModifiedDamageTest_20_Round_Down_30()
        {
            var ability = new DamageTakenModifier
            {
                Modifer = 20f,
                RoundDown = true
            };


            Assert.Equal(10, ability.GetModifiedDamage(30, null));
        }
    }
}
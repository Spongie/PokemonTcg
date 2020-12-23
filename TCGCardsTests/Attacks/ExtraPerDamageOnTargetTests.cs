using Xunit;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class ExtraPerDamageOnTargetTests
    {
        [Fact]
        public void GetDamageTest_No_Damage()
        {
            var attack = new ExtraPerDamageOnTarget()
            {
                Damage = 10,
                ExtraPerDamageCounter = 10
            };

            var target = new PokemonCard();
            Assert.Equal(10, attack.GetDamage(new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }, null).NormalDamage);
        }

        [Fact]
        public void GetDamageTest_20_Damage()
        {
            var attack = new ExtraPerDamageOnTarget()
            {
                Damage = 10,
                ExtraPerDamageCounter = 10
            };

            var target = new PokemonCard() { DamageCounters = 20 };

            Assert.Equal(30, attack.GetDamage(new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }, null).NormalDamage);
        }
    }
}
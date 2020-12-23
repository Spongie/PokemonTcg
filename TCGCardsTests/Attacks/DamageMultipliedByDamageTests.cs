using Xunit;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class DamageMultipliedByDamageTests
    {
        [Fact]
        public void GetDamageTest_No_Damage()
        {
            var attack = new DamageMultipliedByDamage()
            {
                Damage = 10
            };

            var target = new PokemonCard();

            Assert.Equal(0, attack.GetDamage(new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }, null).NormalDamage);
        }

        [Fact]
        public void GetDamageTest_20_Damage()
        {
            var attack = new DamageMultipliedByDamage()
            {
                Damage = 10,
            };

            var target = new PokemonCard() { DamageCounters = 20 };

            Assert.Equal(20, attack.GetDamage(new Player { ActivePokemonCard = target }, new Player { ActivePokemonCard = new PokemonCard() }, null).NormalDamage);
        }
    }
}
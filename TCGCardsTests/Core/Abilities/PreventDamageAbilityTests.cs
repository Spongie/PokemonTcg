using Xunit;

namespace TCGCards.Core.Abilities.Tests
{
    public class PreventDamageAbilityTests
    {
        [Fact]
        public void PreventDamageAbilityTest_Minimum_NotZero_Low_Damage()
        {
            var pokemon = new PokemonCard();
            pokemon.DamageCounters = 20;

            var ability = new PreventDamageAbility(pokemon)
            {
                DamageToPrevent = 9999,
                MinimumBeforePrevention = 30
            };

            ability.Trigger(new Player(), new Player(), 20, new GameField());

            Assert.Equal(20, pokemon.DamageCounters);
        }

        [Fact]
        public void PreventDamageAbilityTest_Minimum_NotZero_High_Damage()
        {
            var pokemon = new PokemonCard();
            pokemon.DamageCounters = 70;

            var ability = new PreventDamageAbility(pokemon)
            {
                DamageToPrevent = 9999,
                MinimumBeforePrevention = 30
            };

            ability.Trigger(new Player(), new Player(), 70, new GameField());

            Assert.Equal(0, pokemon.DamageCounters);
        }

        [Fact]
        public void PreventDamageAbilityTest_Minimum_NotZero_Prevent_Some()
        {
            var pokemon = new PokemonCard();
            pokemon.DamageCounters = 70;

            var ability = new PreventDamageAbility(pokemon)
            {
                DamageToPrevent = 30,
                MinimumBeforePrevention = 30
            };

            ability.Trigger(new Player(), new Player(), 70, new GameField());

            Assert.Equal(40, pokemon.DamageCounters);
        }
    }
}
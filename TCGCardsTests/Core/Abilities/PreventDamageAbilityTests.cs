using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TCGCards.Core.Abilities.Tests
{
    [TestClass()]
    public class PreventDamageAbilityTests
    {
        [TestMethod()]
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

            Assert.AreEqual(20, pokemon.DamageCounters);
        }

        [TestMethod()]
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

            Assert.AreEqual(0, pokemon.DamageCounters);
        }

        [TestMethod()]
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

            Assert.AreEqual(40, pokemon.DamageCounters);
        }
    }
}
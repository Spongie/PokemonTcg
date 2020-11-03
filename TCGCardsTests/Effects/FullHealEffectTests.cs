using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class FullHealEffectTests
    {
        [TestMethod]
        public void FullHeal_CuresAll()
        {
            var pokemon = new PokemonCard();
            var effect = new FullHealEffect()
            {
                TargetingMode = TargetingMode.YourActive
            };

            var player = new Player()
            {
                ActivePokemonCard = pokemon
            };

            pokemon.IsBurned = true;
            pokemon.IsAsleep = true;
            pokemon.IsParalyzed = true;
            pokemon.IsPoisoned = true;
            pokemon.IsConfused = true;

            effect.Process(new GameField(), player, null);

            Assert.IsFalse(pokemon.IsBurned);
            Assert.IsFalse(pokemon.IsAsleep);
            Assert.IsFalse(pokemon.IsParalyzed);
            Assert.IsFalse(pokemon.IsPoisoned);
            Assert.IsFalse(pokemon.IsConfused);
        }

        [TestMethod]
        public void FullHeal_CuresAll_AttachedTo()
        {
            var pokemon = new PokemonCard();
            var effect = new FullHealEffect()
            {
                TargetingMode = TargetingMode.YourActive
            };

            var player = new Player()
            {
                ActivePokemonCard = pokemon
            };

            pokemon.IsBurned = true;
            pokemon.IsAsleep = true;
            pokemon.IsParalyzed = true;
            pokemon.IsPoisoned = true;
            pokemon.IsConfused = true;

            effect.OnAttachedTo(pokemon, true);

            Assert.IsFalse(pokemon.IsBurned);
            Assert.IsFalse(pokemon.IsAsleep);
            Assert.IsFalse(pokemon.IsParalyzed);
            Assert.IsFalse(pokemon.IsPoisoned);
            Assert.IsFalse(pokemon.IsConfused);
        }
    }
}

using Xunit;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    public class FullHealEffectTests
    {
        [Fact]
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

            effect.Process(new GameField(), player, null, null);

            Assert.False(pokemon.IsBurned);
            Assert.False(pokemon.IsAsleep);
            Assert.False(pokemon.IsParalyzed);
            Assert.False(pokemon.IsPoisoned);
            Assert.False(pokemon.IsConfused);
        }

        [Fact]
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

            effect.OnAttachedTo(pokemon, true, null);

            Assert.False(pokemon.IsBurned);
            Assert.False(pokemon.IsAsleep);
            Assert.False(pokemon.IsParalyzed);
            Assert.False(pokemon.IsPoisoned);
            Assert.False(pokemon.IsConfused);
        }
    }
}

using Xunit;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class ApplyEffectTargetTests
    {
        [Fact]
        public void ProcessEffectsTest_Burn()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsBurned);
        }

        [Fact]
        public void ProcessEffectsTest_Confuse()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Confuse
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsConfused);
        }

        [Fact]
        public void ProcessEffectsTest_Paralyze()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Paralyze
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsParalyzed);
        }

        [Fact]
        public void ProcessEffectsTest_Sleep()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Sleep
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsAsleep);
        }

        [Fact]
        public void ProcessEffectsTest_Poison()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Poison
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsPoisoned);
        }
    }
}
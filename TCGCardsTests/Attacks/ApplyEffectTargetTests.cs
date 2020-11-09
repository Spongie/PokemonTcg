using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class ApplyEffectTargetTests
    {
        [TestMethod()]
        public void ProcessEffectsTest_Burn()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsBurned);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Confuse()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Confuse
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsConfused);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Paralyze()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Paralyze
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsParalyzed);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Sleep()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Sleep
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsAsleep);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Poison()
        {
            var attack = new ApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Poison
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsPoisoned);
        }
    }
}
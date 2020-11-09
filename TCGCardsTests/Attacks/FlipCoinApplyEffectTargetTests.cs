using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class FlipCoinApplyEffectTargetTests
    {
        [TestMethod()]
        public void ProcessEffectsTest_Burn()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsBurned);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Confuse()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Confuse
            };

            var target = new PokemonCard();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsConfused);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Paralyze()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Paralyze
            };

            var target = new PokemonCard();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsParalyzed);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Sleep()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Sleep
            };

            var target = new PokemonCard();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsAsleep);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Poison()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Poison
            };

            var target = new PokemonCard();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsTrue(target.IsPoisoned);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Tails()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Poison
            };

            var target = new PokemonCard();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            attack.ProcessEffects(new GameField(), null, new Player { ActivePokemonCard = target });

            Assert.IsFalse(target.IsPoisoned);
        }
    }
}
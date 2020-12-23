using Xunit;
using Entities;
using TCGCards.Core;
using System;

namespace TCGCards.Attacks.Tests
{
    public class FlipCoinApplyEffectTargetTests
    {
        [Fact]
        public void ProcessEffectsTest_Burn()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField().WithFlips(CoinFlipper.HEADS), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsBurned);
        }

        [Fact]
        public void ProcessEffectsTest_Confuse()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Confuse
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField().WithFlips(CoinFlipper.HEADS), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsConfused);
        }

        [Fact]
        public void ProcessEffectsTest_Paralyze()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Paralyze
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField().WithFlips(CoinFlipper.HEADS), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsParalyzed);
        }

        [Fact]
        public void ProcessEffectsTest_Sleep()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Sleep
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField().WithFlips(CoinFlipper.HEADS), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsAsleep);
        }

        [Fact]
        public void ProcessEffectsTest_Poison()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Poison
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField().WithFlips(CoinFlipper.HEADS), null, new Player { ActivePokemonCard = target });

            Assert.True(target.IsPoisoned);
        }

        [Fact]
        public void ProcessEffectsTest_Tails()
        {
            var attack = new FlipCoinApplyEffectTarget()
            {
                StatusEffect = StatusEffect.Poison
            };

            var target = new PokemonCard();

            attack.ProcessEffects(new GameField().WithFlips(CoinFlipper.TAILS), null, new Player { ActivePokemonCard = target });

            Assert.False(target.IsPoisoned);
        }
    }
}
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class ApplyStatusEffectTests
    {
        [TestMethod]
        public void AppliesCorrectStatus()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = false,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.OpponentActive
            };


            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsTrue(pokemon.IsBurned);

            pokemon.IsBurned = false;
            effect.StatusEffect = StatusEffect.Confuse;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsTrue(pokemon.IsConfused);

            pokemon.IsConfused = false;
            effect.StatusEffect = StatusEffect.Paralyze;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsTrue(pokemon.IsParalyzed);

            pokemon.IsParalyzed = false;
            effect.StatusEffect = StatusEffect.Poison;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsTrue(pokemon.IsPoisoned);

            pokemon.IsPoisoned = false;
            effect.StatusEffect = StatusEffect.Sleep;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsTrue(pokemon.IsAsleep);
        }

        [TestMethod]
        public void CoinFlip_AppliesOnHeads()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = true,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.OpponentActive
            };

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            effect.Process(new GameField(), new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsTrue(pokemon.IsBurned);
        }

        [TestMethod]
        public void CoinFlip_FailsOnTails()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = true,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.OpponentActive
            };

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            effect.Process(new GameField(), new Player(), new Player { ActivePokemonCard = pokemon });
            Assert.IsFalse(pokemon.IsBurned);
        }

        [TestMethod]
        public void OnAttachedTo()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = false,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.AttachedTo
            };

            effect.OnAttachedTo(pokemon, true, null);
            Assert.IsTrue(pokemon.IsBurned);
        }
    }
}

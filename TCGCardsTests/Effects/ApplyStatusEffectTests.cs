using Entities;
using Xunit;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    public class ApplyStatusEffectTests
    {
        [Fact]
        public void AppliesCorrectStatus()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = false,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.OpponentActive
            };


            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.True(pokemon.IsBurned);

            pokemon.IsBurned = false;
            effect.StatusEffect = StatusEffect.Confuse;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.True(pokemon.IsConfused);

            pokemon.IsConfused = false;
            effect.StatusEffect = StatusEffect.Paralyze;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.True(pokemon.IsParalyzed);

            pokemon.IsParalyzed = false;
            effect.StatusEffect = StatusEffect.Poison;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.True(pokemon.IsPoisoned);

            pokemon.IsPoisoned = false;
            effect.StatusEffect = StatusEffect.Sleep;
            effect.Process(null, new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.True(pokemon.IsAsleep);
        }

        [Fact]
        public void CoinFlip_AppliesOnHeads()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = true,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.OpponentActive
            };

            effect.Process(new GameField().WithFlips(CoinFlipper.HEADS), new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.True(pokemon.IsBurned);
        }

        [Fact]
        public void CoinFlip_FailsOnTails()
        {
            var pokemon = new PokemonCard();
            var effect = new ApplyStatusEffect
            {
                FlipCoin = true,
                StatusEffect = StatusEffect.Burn,
                TargetingMode = TargetingMode.OpponentActive
            };

            effect.Process(new GameField().WithFlips(CoinFlipper.TAILS), new Player(), new Player { ActivePokemonCard = pokemon }, null);
            Assert.False(pokemon.IsBurned);
        }

        [Fact]
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
            Assert.True(pokemon.IsBurned);
        }
    }
}

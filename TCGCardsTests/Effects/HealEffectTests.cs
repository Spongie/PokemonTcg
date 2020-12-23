using Xunit;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    public class HealEffectTests
    {
        [Fact]
        public void HealApplied()
        {
            var effect = new HealEffect
            {
                Amount = 10,
                TargetingMode = TargetingMode.YourActive
            };

            var player = new Player();
            player.ActivePokemonCard = new PokemonCard() { DamageCounters = 30, Hp = 100 };

            effect.Process(new GameField(), player, null, null);

            Assert.Equal(20, player.ActivePokemonCard.DamageCounters);
        }

        [Fact]
        public void HealApplied_HealthNotNegative()
        {
            var effect = new HealEffect
            {
                Amount = 1000,
                TargetingMode = TargetingMode.YourActive
            };

            var player = new Player();
            player.ActivePokemonCard = new PokemonCard() { DamageCounters = 30, Hp = 100 };

            effect.Process(new GameField(), player, null, null);

            Assert.Equal(0, player.ActivePokemonCard.DamageCounters);
        }
    }
}

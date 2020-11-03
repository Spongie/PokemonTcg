using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class HealEffectTests
    {
        [TestMethod]
        public void HealApplied()
        {
            var effect = new HealEffect
            {
                Amount = 10,
                TargetingMode = TargetingMode.YourActive
            };

            var player = new Player();
            player.ActivePokemonCard = new PokemonCard() { DamageCounters = 30, Hp = 100 };

            effect.Process(new GameField(), player, null);

            Assert.AreEqual(20, player.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void HealApplied_HealthNotNegative()
        {
            var effect = new HealEffect
            {
                Amount = 1000,
                TargetingMode = TargetingMode.YourActive
            };

            var player = new Player();
            player.ActivePokemonCard = new PokemonCard() { DamageCounters = 30, Hp = 100 };

            effect.Process(new GameField(), player, null);

            Assert.AreEqual(0, player.ActivePokemonCard.DamageCounters);
        }
    }
}

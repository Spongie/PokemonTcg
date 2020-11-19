using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class DamageEffectTests
    {
        [TestMethod]
        public void DamageAppliedToSelf()
        {
            var effect = new DamageEffect()
            {
                Amount = 10
            };

            var pokemon = new PokemonCard();
            effect.OnAttachedTo(pokemon, true, null);

            Assert.AreEqual(10, pokemon.DamageCounters);
        }
    }
}

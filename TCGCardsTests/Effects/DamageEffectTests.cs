using Xunit;
using TCGCards;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    public class DamageEffectTests
    {
        [Fact]
        public void DamageAppliedToSelf()
        {
            var effect = new DamageEffect()
            {
                Amount = 10
            };

            var pokemon = new PokemonCard();
            effect.OnAttachedTo(pokemon, true, null);

            Assert.Equal(10, pokemon.DamageCounters);
        }
    }
}

using Xunit;
using TCGCards;
using TCGCards.TrainerEffects;
using TCGCards.Core;

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

            var pokemon = new PokemonCard() { Owner = new Player() };
            effect.OnAttachedTo(pokemon, true, null);

            Assert.Equal(10, pokemon.DamageCounters);
        }
    }
}

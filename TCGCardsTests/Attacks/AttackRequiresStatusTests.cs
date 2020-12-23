using Xunit;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class AttackRequiresStatusTests
    {
        [Fact]
        public void CanBeUsedTest_Wrong_Status()
        {
            var attack = new AttackRequiresStatus()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard() { IsAsleep = true };

            Assert.False(attack.CanBeUsed(new GameField { FirstTurn = false }, new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }));
        }

        [Fact]
        public void CanBeUsedTest_No_Status()
        {
            var attack = new AttackRequiresStatus()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard() {  };

            Assert.False(attack.CanBeUsed(new GameField { FirstTurn = false }, new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }));
        }

        [Fact]
        public void CanBeUsedTest_Correct_Status()
        {
            var attack = new AttackRequiresStatus()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard() { IsBurned = true };

            Assert.True(attack.CanBeUsed(new GameField { FirstTurn = false }, new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }));
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class AttackRequiresStatusTests
    {
        [TestMethod()]
        public void CanBeUsedTest_Wrong_Status()
        {
            var attack = new AttackRequiresStatus()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard() { IsAsleep = true };

            Assert.IsFalse(attack.CanBeUsed(new GameField { FirstTurn = false }, new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }));
        }

        [TestMethod()]
        public void CanBeUsedTest_No_Status()
        {
            var attack = new AttackRequiresStatus()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard() {  };

            Assert.IsFalse(attack.CanBeUsed(new GameField { FirstTurn = false }, new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }));
        }

        [TestMethod()]
        public void CanBeUsedTest_Correct_Status()
        {
            var attack = new AttackRequiresStatus()
            {
                StatusEffect = StatusEffect.Burn
            };

            var target = new PokemonCard() { IsBurned = true };

            Assert.IsTrue(attack.CanBeUsed(new GameField { FirstTurn = false }, new Player { ActivePokemonCard = new PokemonCard() }, new Player { ActivePokemonCard = target }));
        }
    }
}
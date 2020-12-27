using Xunit;
using TCGCards.Core.Abilities;

namespace TCGCards.Core.Tests
{
    public class AbilityTests
    {
        [Fact]
        public void CanActivate_Stadium()
        {
            var card = new TrainerCard();
            card.Ability = new RetreatCostModifierAbility();

            var game = new GameField();
            game.InitTest();

            Assert.True(card.Ability.CanActivate(game, game.ActivePlayer, game.NonActivePlayer));
        }
    }
}
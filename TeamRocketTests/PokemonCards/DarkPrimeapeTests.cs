using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.PokemonCards
{
    [TestClass]
    public class DarkPrimeapeTests
    {
        [TestMethod]
        public void FrenzyAbility_Triggers()
        {
            var gameField = createGameField();
            gameField.ActivePlayer.ActivePokemonCard.IsConfused = true;
            CoinFlipper.ForcedNextFlips.Enqueue(false);

            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[0]);

            Assert.AreEqual(GameField.ConfusedDamage + 30, gameField.ActivePlayer.ActivePokemonCard.DamageCounters);
        }

        private GameField createGameField()
        {
            var gameField = new GameField();
            gameField.InitTest();

            gameField.ActivePlayer.ActivePokemonCard = new DarkPrimeape(gameField.ActivePlayer);
            gameField.NonActivePlayer.ActivePokemonCard = new DarkPrimeape(gameField.NonActivePlayer);

            return gameField;
        }
    }
}

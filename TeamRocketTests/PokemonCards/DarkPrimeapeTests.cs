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

            gameField.ActivePlayer.ActivePokemonCard.DamageCounters = -100000; //So it does not die :)

            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[0]);

            Assert.AreEqual(-100000 + GameField.ConfusedDamage + 30, gameField.NonActivePlayer.ActivePokemonCard.DamageCounters);
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

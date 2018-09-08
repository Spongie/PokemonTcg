using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.Core;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.PokemonCards
{
    [TestClass]
    public class DarkJolteonTests
    {
        [TestMethod]
        public void LightningFlashEffectAppliedOnOpponent()
        {
            var gameField = createGameField();
            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[0]);

            Assert.IsTrue(gameField.ActivePlayer.ActivePokemonCard.AttackStoppers.Any());
        }

        [TestMethod]
        public void LightningFlashEffectAppliedOnOpponent_AndActivated()
        {
            var gameField = createGameField();
            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[0]);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[1]);
            Assert.AreEqual(0, gameField.ActivePlayer.ActivePokemonCard.DamageCounters);
        }

        private GameField createGameField()
        {
            var gameField = new GameField();
            gameField.InitTest();

            gameField.ActivePlayer.ActivePokemonCard = new DarkJolteon(gameField.ActivePlayer);
            gameField.NonActivePlayer.ActivePokemonCard = new DarkJolteon(gameField.NonActivePlayer);

            return gameField;
        }
    }
}

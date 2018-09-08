using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.Core;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.PokemonCards
{
    [TestClass]
    public class EeveeTests
    {
        [TestMethod]
        public void SandAttackEffectAppliedOnOpponent()
        {
            var gameField = createGameField();
            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[1]);

            Assert.IsTrue(gameField.ActivePlayer.ActivePokemonCard.AttackStoppers.Any());
        }

        [TestMethod]
        public void SandAttackEffectAppliedOnOpponent_AndActivated()
        {
            var gameField = createGameField();
            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[1]);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[0]);
            Assert.AreEqual(0, gameField.ActivePlayer.ActivePokemonCard.DamageCounters);
        }

        private GameField createGameField()
        {
            var gameField = new GameField();
            gameField.InitTest();

            gameField.ActivePlayer.ActivePokemonCard = new Eevee(gameField.ActivePlayer);
            gameField.NonActivePlayer.ActivePokemonCard = new Eevee(gameField.NonActivePlayer);

            return gameField;
        }
    }
}

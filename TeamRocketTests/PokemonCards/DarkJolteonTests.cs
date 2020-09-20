using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.Core;
using TCGCards.EnergyCards;
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

            CoinFlipper.ForcedNextFlips.Clear();
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            gameField.Attack(gameField.ActivePlayer.ActivePokemonCard.Attacks[0]);
            Assert.AreEqual(0, gameField.ActivePlayer.ActivePokemonCard.DamageCounters);
        }

        private GameField createGameField()
        {
            var gameField = new GameField();
            gameField.InitTest();
            gameField.FirstTurn = false;

            gameField.ActivePlayer.ActivePokemonCard = new DarkJolteon(gameField.ActivePlayer);
            gameField.NonActivePlayer.ActivePokemonCard = new DarkJolteon(gameField.NonActivePlayer);

            gameField.ActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new LightningEnergy());
            gameField.ActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new LightningEnergy());
            gameField.ActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new LightningEnergy());

            gameField.NonActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new LightningEnergy());
            gameField.NonActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new LightningEnergy());
            gameField.NonActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new LightningEnergy());

            return gameField;
        }
    }
}

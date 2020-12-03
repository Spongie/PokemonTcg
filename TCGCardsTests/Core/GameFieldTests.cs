using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace TCGCards.Core.Tests
{
    [TestClass()]
    public class GameFieldTests
    {
        [TestMethod()]
        public void IsSuccessfulFlipTest_Default()
        {
            var game = new GameField();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            Assert.IsTrue(game.IsSuccessfulFlip(true, false, false));

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            Assert.IsFalse(game.IsSuccessfulFlip(true, false, false));
        }

        [TestMethod()]
        public void IsSuccessfulFlipTest_Check_Tails()
        {
            var game = new GameField();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            Assert.IsFalse(game.IsSuccessfulFlip(true, false, true));

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            Assert.IsTrue(game.IsSuccessfulFlip(true, false, true));
        }

        [TestMethod()]
        public void IsSuccessfulFlipTest_CheckLast_Heads()
        {
            var game = new GameField();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            game.FlipCoins(1);

            Assert.IsTrue(game.IsSuccessfulFlip(false, true, false));

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            game.FlipCoins(1);

            Assert.IsFalse(game.IsSuccessfulFlip(false, true, false));
        }

        [TestMethod()]
        public void IsSuccessfulFlipTest_CheckLast_Tails()
        {
            var game = new GameField();

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            game.FlipCoins(1);

            Assert.IsTrue(game.IsSuccessfulFlip(false, true, true));

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            game.FlipCoins(1);
            Assert.IsFalse(game.IsSuccessfulFlip(false, true, true));
        }
    }
}
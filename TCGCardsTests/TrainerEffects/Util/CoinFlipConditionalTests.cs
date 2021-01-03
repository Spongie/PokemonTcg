using TCGCards.TrainerEffects.Util;
using Xunit;
using TCGCards.Core;
using Entities;

namespace TCGCards.TrainerEffects.Util.Tests
{
    public class CoinFlipConditionalTests
    {
        [Fact]
        public void IsOk_Empty()
        {
            var condition = new CoinFlipConditional();

            Assert.True(condition.IsOk(new GameField(), new Player()));
        }

        [Fact]
        public void IsOk_No_Flip()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = false
            };

            Assert.True(condition.IsOk(new GameField(), new Player()));
        }

        [Fact]
        public void IsOk_Heads_Flip()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true
            };

            Assert.True(condition.IsOk(new GameField().WithFlips(CoinFlipper.HEADS), new Player()));
        }

        [Fact]
        public void IsOk_Tails_Flip()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true
            };

            Assert.False(condition.IsOk(new GameField().WithFlips(CoinFlipper.TAILS), new Player()));
        }

        [Fact]
        public void IsOk_Tails_Flip_Check_Tails()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                CheckTails = true
            };

            Assert.True(condition.IsOk(new GameField().WithFlips(CoinFlipper.TAILS), new Player()));
        }

        [Fact]
        public void IsOk_Flip_2_1_Heads_1_Required()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                CoinsToFlip = 2,
                SuccessessForBonus = 1
            };

            Assert.True(condition.IsOk(new GameField().WithFlips(CoinFlipper.HEADS, CoinFlipper.TAILS), new Player()));
        }

        [Fact]
        public void IsOk_Flip_2_1_Heads_2_Required()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                CoinsToFlip = 2,
                SuccessessForBonus = 2
            };

            Assert.False(condition.IsOk(new GameField().WithFlips(CoinFlipper.HEADS, CoinFlipper.TAILS), new Player()));
        }

        [Fact]
        public void IsOk_Flip_2_2_Heads_1_Required()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                CoinsToFlip = 2,
                SuccessessForBonus = 2
            };

            Assert.True(condition.IsOk(new GameField().WithFlips(CoinFlipper.HEADS, CoinFlipper.HEADS), new Player()));
        }

        [Fact]
        public void IsOk_Flip_2_Check_2_Tails()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                CoinsToFlip = 2,
                CheckTails = true,
                SuccessessForBonus = 2
            };

            Assert.True(condition.IsOk(new GameField().WithFlips(CoinFlipper.TAILS, CoinFlipper.TAILS), new Player()));
        }

        [Fact]
        public void IsOk_Flip_2_Check_2_Tails_1_Heads()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                CoinsToFlip = 2,
                CheckTails = true,
                SuccessessForBonus = 2
            };

            Assert.False(condition.IsOk(new GameField().WithFlips(CoinFlipper.HEADS, CoinFlipper.TAILS), new Player()));
        }

        [Fact]
        public void IsOk_Last_Flip_Heads()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                UseLastCoin = true
            };

            Assert.True(condition.IsOk(new GameField() { LastCoinFlipResult = CoinFlipper.HEADS, LastCoinFlipHeadCount = 1 }, new Player()));
        }

        [Fact]
        public void IsOk_Last_Flip_Heads_Check_Tails()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                UseLastCoin = true,
                CheckTails = true
            };

            Assert.False(condition.IsOk(new GameField() { LastCoinFlipResult = CoinFlipper.HEADS, LastCoinFlipHeadCount = 1 }, new Player()));
        }

        [Fact]
        public void IsOk_Last_Flip_Tails_Check_Tails()
        {
            var condition = new CoinFlipConditional()
            {
                FlipCoin = true,
                UseLastCoin = true,
                CheckTails = true
            };

            Assert.True(condition.IsOk(new GameField() { LastCoinFlipResult = CoinFlipper.TAILS, LastCoinFlipHeadCount = 0 }, new Player()));
        }

        [Fact]
        public void IsOk_Double_Tails_First_Second_Check_Last()
        {
            var game = new GameField().WithFlips(CoinFlipper.TAILS, CoinFlipper.TAILS);

            var conditional = new CoinFlipConditional
            {
                FlipCoin = true,
                CoinsToFlip = 2,
                SuccessessForBonus = 2,
                CheckTails = true
            };

            Assert.True(conditional.IsOk(game, null));

            var secondCondition = new CoinFlipConditional
            {
                UseLastCoin = true
            };

            Assert.True(secondCondition.IsOk(game, null));
        }
    }
}
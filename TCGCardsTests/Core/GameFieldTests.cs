using Xunit;
using Entities;

namespace TCGCards.Core.Tests
{
    public class GameFieldTests
    {
        [Fact]
        public void IsSuccessfulFlipTest_Default()
        {
            var game = new GameField().WithFlips(CoinFlipper.HEADS);

            Assert.True(game.IsSuccessfulFlip(true, false, false));

            game.WithFlips(CoinFlipper.TAILS);
            Assert.False(game.IsSuccessfulFlip(true, false, false));
        }

        [Fact]
        public void IsSuccessfulFlipTest_Check_Tails()
        {
            var game = new GameField().WithFlips(CoinFlipper.HEADS);

            Assert.False(game.IsSuccessfulFlip(true, false, true));

            game.WithFlips(CoinFlipper.TAILS);
            Assert.True(game.IsSuccessfulFlip(true, false, true));
        }

        [Fact]
        public void IsSuccessfulFlipTest_CheckLast_Heads()
        {
            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            game.FlipCoins(1);

            Assert.True(game.IsSuccessfulFlip(false, true, false));

            game.WithFlips(CoinFlipper.TAILS);
            game.FlipCoins(1);

            Assert.False(game.IsSuccessfulFlip(false, true, false));
        }

        [Fact]
        public void IsSuccessfulFlipTest_CheckLast_Tails()
        {
            var game = new GameField().WithFlips(CoinFlipper.TAILS);
            game.FlipCoins(1);

            Assert.True(game.IsSuccessfulFlip(false, true, true));

            game.WithFlips(CoinFlipper.HEADS);
            game.FlipCoins(1);
            Assert.False(game.IsSuccessfulFlip(false, true, true));
        }

        [Fact()]
        public void SwapActivePlayerTest()
        {
            Player p1 = new Player();
            Player p2 = new Player();
            var game = new GameField();
            game.AddPlayer(p1);
            game.AddPlayer(p2);
            game.ActivePlayerIndex = 0;

            Assert.Equal(0, game.ActivePlayerIndex);
            Assert.Equal(p1, game.ActivePlayer);
            Assert.Equal(p2, game.NonActivePlayer);

            game.SwapActivePlayer();

            Assert.Equal(1, game.ActivePlayerIndex);
            Assert.Equal(p1, game.NonActivePlayer);
            Assert.Equal(p2, game.ActivePlayer);

            game.SwapActivePlayer();

            Assert.Equal(0, game.ActivePlayerIndex);
            Assert.Equal(p1, game.ActivePlayer);
            Assert.Equal(p2, game.NonActivePlayer);
        }
    }
}
using Xunit;
using TCGCards.Attacks;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using Entities;

namespace TCGCards.Attacks.Tests
{
    public class FlipCoinPlusAttackTests
    {
        [Fact]
        public void GetDamage_1_Flip_Heads()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 1,
                HeadsForBonus = 1,
                ExtraforHeads = 20,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            Assert.Equal(40, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_1_Flip_Tails()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 1,
                HeadsForBonus = 1,
                ExtraforHeads = 20,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.TAILS);
            Assert.Equal(20, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_1_Flip_Tails_Bonus()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 1,
                HeadsForBonus = 1,
                ExtraforHeads = 20,
                ExtraforTails = 10,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.TAILS);
            Assert.Equal(30, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_2_Flip_2_Heads()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 2,
                HeadsForBonus = 2,
                ExtraforHeads = 40,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.HEADS, CoinFlipper.HEADS);
            Assert.Equal(60, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_2_Flip_1_Heads()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 2,
                HeadsForBonus = 2,
                ExtraforHeads = 40,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.TAILS, CoinFlipper.HEADS);
            Assert.Equal(20, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_2_Flip_0_Heads()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 2,
                HeadsForBonus = 2,
                ExtraforHeads = 40,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.TAILS, CoinFlipper.TAILS);
            Assert.Equal(20, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_2_Flip_1_Heads_Only_1_Required()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 2,
                HeadsForBonus = 1,
                ExtraforHeads = 40,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.TAILS, CoinFlipper.HEADS);
            Assert.Equal(60, attack.GetDamage(null, null, game).NormalDamage);
        }

        [Fact]
        public void GetDamage_2_Flip_2_Heads_Only_1_Required()
        {
            var attack = new FlipCoinPlusAttack()
            {
                CoinsToFlip = 2,
                HeadsForBonus = 1,
                ExtraforHeads = 40,
                Damage = 20
            };

            var game = new GameField().WithFlips(CoinFlipper.HEADS, CoinFlipper.HEADS);
            Assert.Equal(60, attack.GetDamage(null, null, game).NormalDamage);
        }
    }
}
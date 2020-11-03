using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class CardDrawEffectTests
    {
        [TestMethod]
        public void CardDrawNoFlip()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = false,
                Opponents = false
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());

            effect.Process(new GameField(), player, null);
            Assert.AreEqual(0, player.Deck.Cards.Count);
            Assert.AreEqual(1, player.Hand.Count);
        }

        [TestMethod]
        public void CardDrawFlipTails()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = true,
                Opponents = false
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            effect.Process(new GameField(), player, null);
            Assert.AreEqual(1, player.Deck.Cards.Count);
            Assert.AreEqual(0, player.Hand.Count);
        }

        [TestMethod]
        public void CardDrawFlipHeads()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = true,
                Opponents = false
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            effect.Process(new GameField(), player, null);
            Assert.AreEqual(0, player.Deck.Cards.Count);
            Assert.AreEqual(1, player.Hand.Count);
        }

        [TestMethod]
        public void CardDrawNoFlip_Opponent()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = false,
                Opponents = true
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());

            effect.Process(new GameField(), null, player);
            Assert.AreEqual(0, player.Deck.Cards.Count);
            Assert.AreEqual(1, player.Hand.Count);
        }

        [TestMethod]
        public void CardDrawFlipTails_Opponent()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = true,
                Opponents = true
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            effect.Process(new GameField(), null, player);
            Assert.AreEqual(1, player.Deck.Cards.Count);
            Assert.AreEqual(0, player.Hand.Count);
        }

        [TestMethod]
        public void CardDrawFlipHeads_Opponent()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = true,
                Opponents = true
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            effect.Process(new GameField(), null, player);
            Assert.AreEqual(0, player.Deck.Cards.Count);
            Assert.AreEqual(1, player.Hand.Count);
        }

        [TestMethod]
        public void OnAttachedTo()
        {
            var effect = new CardDrawEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = false,
                Opponents = false
            };

            var player = new Player();
            player.Deck.Cards.Push(new PokemonCard());
            var pokemon = new PokemonCard() { Owner = player };
            effect.OnAttachedTo(pokemon, true);
            Assert.AreEqual(1, player.Hand.Count);
        }
    }
}

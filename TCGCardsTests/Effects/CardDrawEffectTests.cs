using Entities;
using Xunit;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    public class CardDrawEffectTests
    {
        [Fact]
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

            effect.Process(new GameField(), player, null, null);
            Assert.Empty(player.Deck.Cards);
            Assert.Single(player.Hand);
        }

        [Fact]
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
            effect.Process(new GameField().WithFlips(CoinFlipper.TAILS), player, null, null);
            Assert.Single(player.Deck.Cards);
            Assert.Empty(player.Hand);
        }

        [Fact]
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
            effect.Process(new GameField().WithFlips(CoinFlipper.HEADS), player, null, null);
            Assert.Empty(player.Deck.Cards);
            Assert.Single(player.Hand);
        }

        [Fact]
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

            effect.Process(new GameField(), null, player, null);
            Assert.Empty(player.Deck.Cards);
            Assert.Single(player.Hand);
        }

        [Fact]
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
            effect.Process(new GameField().WithFlips(CoinFlipper.TAILS), null, player, null);
            Assert.Single(player.Deck.Cards);
            Assert.Empty(player.Hand);
        }

        [Fact]
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
            effect.Process(new GameField().WithFlips(CoinFlipper.HEADS), null, player, null);
            Assert.Empty(player.Deck.Cards);
            Assert.Single(player.Hand);
        }

        [Fact]
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
            effect.OnAttachedTo(pokemon, true, null);
            Assert.Single(player.Hand);
        }
    }
}

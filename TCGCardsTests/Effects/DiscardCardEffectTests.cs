using Entities;
using Xunit;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects;
using TCGCards.TrainerEffects.Util;

namespace TCGCardsTests.Effects
{
    public class DiscardCardEffectTests
    {
        [Fact]
        public void DiscardAny_NoFlip()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { player.Hand[0].Id } });
            player.SetNetworkPlayer(sub);

            effect.Process(new GameField(), player, null, null);

            Assert.Single(player.DiscardPile);
            Assert.Equal(2, player.Hand.Count);
        }

        [Fact]
        public void DiscardAny_Flip_Tails()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                CoinflipConditional = new CoinFlipConditional
                {
                    FlipCoin = true
                },
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { player.Hand[0].Id } });
            player.SetNetworkPlayer(sub);
            effect.Process(new GameField().WithFlips(CoinFlipper.TAILS), player, null, null);

            Assert.Empty(player.DiscardPile);
            Assert.Equal(3, player.Hand.Count);
        }

        [Fact]
        public void DiscardAny_Flip_Heads()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                CoinflipConditional = new CoinFlipConditional
                {
                    FlipCoin = true
                },
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { player.Hand[0].Id } });
            player.SetNetworkPlayer(sub);
            effect.Process(new GameField().WithFlips(CoinFlipper.HEADS), player, null, null);

            Assert.Single(player.DiscardPile);
            Assert.Equal(2, player.Hand.Count);
        }

        [Fact]
        public void DiscardAllCards()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = -1,
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            effect.Process(new GameField(), player, null, null);

            Assert.Equal(3, player.DiscardPile.Count);
            Assert.Empty(player.Hand);
        }

        [Fact]
        public void DiscardAny_OnlyTrainer()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                CardType = CardType.Any
            };

            var player = new Player();
            var target = new TrainerCard();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(target);

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { target.Id } });
            player.SetNetworkPlayer(sub);

            effect.Process(new GameField(), player, null, null);

            Assert.Single(player.DiscardPile);
            Assert.Equal(2, player.Hand.Count);
            Assert.Equal(target.Id, player.DiscardPile[0].Id);
        }
    }
}

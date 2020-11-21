using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class DiscardCardEffectTests
    {
        public void DiscardAny_NoFlip()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = false,
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

            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.AreEqual(2, player.Hand.Count);
        }

        [TestMethod]
        public void DiscardAny_Flip_Tails()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = true,
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { player.Hand[0].Id } });
            player.SetNetworkPlayer(sub);
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);
            effect.Process(new GameField(), player, null, null);

            Assert.AreEqual(0, player.DiscardPile.Count);
            Assert.AreEqual(3, player.Hand.Count);
        }

        [TestMethod]
        public void DiscardAny_Flip_Heads()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = true,
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { player.Hand[0].Id } });
            player.SetNetworkPlayer(sub);
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            effect.Process(new GameField(), player, null, null);

            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.AreEqual(2, player.Hand.Count);
        }

        [TestMethod]
        public void DiscardAllCards()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = -1,
                OnlyOnCoinFlip = false,
                CardType = CardType.Any
            };

            var player = new Player();
            player.Hand.Add(new EnergyCard());
            player.Hand.Add(new PokemonCard());
            player.Hand.Add(new TrainerCard());

            effect.Process(new GameField(), player, null, null);

            Assert.AreEqual(3, player.DiscardPile.Count);
            Assert.AreEqual(0, player.Hand.Count);
        }

        [TestMethod]
        public void DiscardAny_OnlyTrainer()
        {
            var effect = new DiscardCardEffect()
            {
                Amount = 1,
                OnlyOnCoinFlip = false,
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

            Assert.AreEqual(1, player.DiscardPile.Count);
            Assert.AreEqual(2, player.Hand.Count);
            Assert.AreEqual(target.Id, player.DiscardPile[0].Id);
        }
    }
}

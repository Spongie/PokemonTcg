﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects;

namespace TCGCardsTests.Effects
{
    [TestClass]
    public class SearchDeckForCardEffectTests
    {
        [TestMethod]
        public void DeckSearched()
        {
            var effect = new SearchDeckForCardEffect()
            {
                CardType = CardType.Any
            };

            var player = new Player();
            var target = new EnergyCard();
            player.Deck.Cards.Push(target);
            player.Deck.Cards.Push(new EnergyCard());
            player.Deck.Cards.Push(new EnergyCard());
            player.Deck.Cards.Push(new EnergyCard());
            player.Deck.Cards.Push(new EnergyCard());

            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage { Cards = new List<NetworkId> { target.Id } });
            player.SetNetworkPlayer(sub);

            var game = new GameField();
            game.Players.Add(player);
            effect.Process(game, player, null);

            Assert.AreEqual(1, player.Hand.Count);
            Assert.AreEqual(4, player.Deck.Cards.Count);
            Assert.AreEqual(target.Id, player.Hand[0].Id);
        }
    }
}
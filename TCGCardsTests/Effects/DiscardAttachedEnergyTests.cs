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
    public class DiscardAttachedEnergyTests
    {
        [TestMethod]
        public void AttachedAmountSameAsDiscard()
        {
            var effect = new DiscardAttachedEnergy
            {
                Amount = 1,
                TargetingMode = TargetingMode.OpponentActive
            };

            var opponent = new Player();
            var player = new Player();
            opponent.ActivePokemonCard = new PokemonCard(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());

            effect.Process(new GameField(), player, opponent, null);

            Assert.AreEqual(0, opponent.ActivePokemonCard.AttachedEnergy.Count);
            Assert.AreEqual(1, opponent.DiscardPile.Count);
        }

        [TestMethod]
        public void AttachedAmountMoreThanDiscard()
        {
            var effect = new DiscardAttachedEnergy
            {
                Amount = 1,
                TargetingMode = TargetingMode.OpponentActive,
            };

            var opponent = new Player();
            var player = new Player();
            opponent.ActivePokemonCard = new PokemonCard(opponent);
            var goodCard = new EnergyCard();

            INetworkPlayer sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>())
                .ReturnsForAnyArgs(new CardListMessage() { Cards = new List<NetworkId> { goodCard.Id } });

            player.SetNetworkPlayer(sub);

            opponent.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());
            opponent.ActivePokemonCard.AttachedEnergy.Add(goodCard);

            effect.Process(new GameField(), player, opponent, null);

            Assert.AreEqual(1, opponent.ActivePokemonCard.AttachedEnergy.Count);
            Assert.AreEqual(goodCard.Id, opponent.DiscardPile[0].Id);
            Assert.AreEqual(1, opponent.DiscardPile.Count);
        }
    }
}

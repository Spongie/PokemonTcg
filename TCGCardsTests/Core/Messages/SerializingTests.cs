using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCardsTests.Core.Messages
{
    [TestClass]
    public class SerializingTests
    {
        [TestMethod]
        public void AttachedEnergyDoneMessage()
        {
            var message = new AttachedEnergyDoneMessage(new Dictionary<NetworkId, NetworkId>());

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void AttachEnergyCardsToBench()
        {
            var message = new AttachEnergyCardsToBenchMessage(new List<EnergyCard>() { new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 } });

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void CardListMessage()
        {
            var message = new CardListMessage(new List<NetworkId>() { NetworkId.Generate(), NetworkId.Generate(), NetworkId.Generate() });

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void DeckSearchMessage()
        {
            var message = new DeckSearchMessage(new Deck(), new List<IDeckFilter>(), 1);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void DiscardCardsMessage()
        {
            var message = new DiscardCardsMessage(2);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void GameFieldMessage()
        {
            var message = new GameFieldMessage(new GameField());

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void GameLogAddMessage()
        {
            var message = new GameLogAddMessage(new List<string> { "asd", "dskaj" });

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void GameLogReloadMessage()
        {
            var message = new GameLogReloadMessage(new GameLog());

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void GameOverMessage()
        {
            var message = new GameOverMessage(NetworkId.Generate());

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void InfoMessage()
        {
            var message = new InfoMessage("asdasd");

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void PickFromListMessage()
        {
            var message = new PickFromListMessage(new List<Card> { new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 } }, 1);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void PickFromListMessageDeserialize()
        {
            var player = new Player();
            var message = new PickFromListMessage(new List<Card> { new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 }, new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 } }, 1, 1);

            var result = SerializeAndBack(message);

            Assert.AreEqual(1, result.MinCount);
            Assert.AreEqual(1, result.MaxCount);
            Assert.AreEqual(2, result.PossibleChoices.Count);
        }

        [TestMethod]
        public void SelectAttackMessage()
        {
            var message = new SelectAttackMessage(new List<Attack>(new List<Attack>()));

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void SelectColorMessage()
        {
            var message = new SelectColorMessage(EnergyTypes.Colorless);

            var data = SerializeAndBack(message);
            Assert.AreEqual(EnergyTypes.Colorless, data.Color);
        }

        [TestMethod]
        public void SelectFromOpponentBenchMessage()
        {
            var message = new SelectFromOpponentBenchMessage(1);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void SelectFromYourBenchMessage()
        {
            var message = new SelectFromYourBenchMessage(2);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void SelectFromYourPokemonMessage()
        {
            var message = new SelectFromYourPokemonMessage("asd", EnergyTypes.Darkness);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void SelectOpponentPokemonMessage()
        {
            var message = new SelectOpponentPokemonMessage(3);

            Assert.IsNotNull(SerializeAndBack(message));
        }

        [TestMethod]
        public void SelectPrizeCardsMessage()
        {
            var message = new SelectPrizeCardsMessage(1);

            var data = SerializeAndBack(message);
            Assert.AreEqual(1, data.Amount);
        }

        private T SerializeAndBack<T>(T message)
        {
            var json = Serializer.Serialize(message);

            return Serializer.Deserialize<T>(json);
        }
    }
}

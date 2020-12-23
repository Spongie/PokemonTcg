using Entities;
using Xunit;
using NetworkingCore;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCardsTests.Core.Messages
{
    public class SerializingTests
    {
        [Fact]
        public void AttachedEnergyDoneMessage()
        {
            var message = new AttachedEnergyDoneMessage(new Dictionary<NetworkId, NetworkId>());

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void AttachEnergyCardsToBench()
        {
            var message = new AttachEnergyCardsToBenchMessage(new List<EnergyCard>() { new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 } });

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void CardListMessage()
        {
            var message = new CardListMessage(new List<NetworkId>() { NetworkId.Generate(), NetworkId.Generate(), NetworkId.Generate() });

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void DeckSearchMessage()
        {
            var message = new DeckSearchMessage(new List<Card>(), new List<IDeckFilter>(), 1);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void DiscardCardsMessage()
        {
            var message = new DiscardCardsMessage(2);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void GameFieldMessage()
        {
            var message = new GameFieldMessage(new GameField());

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void GameLogAddMessage()
        {
            var message = new GameLogAddMessage(new List<string> { "asd", "dskaj" });

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void GameLogReloadMessage()
        {
            var message = new GameLogReloadMessage(new GameLog());

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void GameOverMessage()
        {
            var message = new GameOverMessage(NetworkId.Generate());

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void InfoMessage()
        {
            var message = new InfoMessage("asdasd");

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void PickFromListMessage()
        {
            var message = new PickFromListMessage(new List<Card> { new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 } }, 1);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void PickFromListMessageDeserialize()
        {
            var player = new Player();
            var message = new PickFromListMessage(new List<Card> { new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 }, new EnergyCard() { EnergyType = EnergyTypes.Water, Amount = 1 } }, 1, 1);

            var result = SerializeAndBack(message);

            Assert.Equal(1, result.MinCount);
            Assert.Equal(1, result.MaxCount);
            Assert.Equal(2, result.PossibleChoices.Count);
        }

        [Fact]
        public void SelectAttackMessage()
        {
            var message = new SelectAttackMessage(new List<Attack>(new List<Attack>()));

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void SelectColorMessage()
        {
            var message = new SelectColorMessage(EnergyTypes.Colorless);

            var data = SerializeAndBack(message);
            Assert.Equal(EnergyTypes.Colorless, data.Color);
        }

        [Fact]
        public void SelectFromOpponentBenchMessage()
        {
            var message = new SelectFromOpponentBenchMessage(1);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void SelectFromYourBenchMessage()
        {
            var message = new SelectFromYourBenchMessage(2);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void SelectFromYourPokemonMessage()
        {
            var message = new SelectFromYourPokemonMessage("asd", EnergyTypes.Darkness);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void SelectOpponentPokemonMessage()
        {
            var message = new SelectOpponentPokemonMessage(3);

            Assert.NotNull(SerializeAndBack(message));
        }

        [Fact]
        public void SelectPrizeCardsMessage()
        {
            var message = new SelectPrizeCardsMessage(1);

            var data = SerializeAndBack(message);
            Assert.Equal(1, data.Amount);
        }

        private T SerializeAndBack<T>(T message)
        {
            var json = Serializer.Serialize(message);

            return Serializer.Deserialize<T>(json);
        }
    }
}

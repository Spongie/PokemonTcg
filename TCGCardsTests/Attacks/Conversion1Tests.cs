using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using NSubstitute;
using NetworkingCore;
using TCGCards.Core.Messages;
using Entities;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class Conversion1Tests
    {
        [TestMethod()]
        public void ProcessEffectsTest()
        {
            var attack = new Conversion1();

            var player = new Player();
            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<SelectColorMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new SelectColorMessage
            {
                Color = EnergyTypes.Darkness
            });
            player.SetNetworkPlayer(sub);

            var opponent = new Player()
            {
                ActivePokemonCard = new PokemonCard()
            };

            attack.ProcessEffects(new GameField(), player, opponent);

            Assert.AreEqual(EnergyTypes.Darkness, opponent.ActivePokemonCard.Weakness);
        }
    }
}
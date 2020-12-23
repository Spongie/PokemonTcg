using Xunit;
using TCGCards.Core;
using NSubstitute;
using TCGCards.Core.Messages;
using NetworkingCore;
using Entities;

namespace TCGCards.Attacks.Tests
{
    public class Conversion2Tests
    {
        [Fact]
        public void ProcessEffectsTest()
        {
            var attack = new Conversion2();

            var player = new Player();
            var sub = Substitute.For<INetworkPlayer>();
            sub.SendAndWaitForResponse<SelectColorMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new SelectColorMessage
            {
                Color = EnergyTypes.Darkness
            });
            player.SetNetworkPlayer(sub);
            
            player.ActivePokemonCard = new PokemonCard();

            var opponent = new Player()
            {
                ActivePokemonCard = new PokemonCard()
            };

            attack.ProcessEffects(new GameField(), player, opponent);

            Assert.Equal(EnergyTypes.Darkness, player.ActivePokemonCard.Resistance);
        }
    }
}
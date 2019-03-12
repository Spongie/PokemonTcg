using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class SneakAttackTests
    {
        [TestMethod]
        public void Trigger_Test()
        {
            var owner = new Player();

            var opponent = new Player();
            opponent.SetActivePokemon(new DarkGloom(opponent));

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(
                new CardListMessage(       
                    new List<Card>
                    {
                        opponent.ActivePokemonCard
                    }));

            owner.SetNetworkPlayer(networkPlayer);

            var pokemon = new DarkGolbat(owner);

            pokemon.Ability.Trigger(owner, opponent, 0);

            Assert.AreEqual(10, opponent.ActivePokemonCard.DamageCounters);
        }
    }
}

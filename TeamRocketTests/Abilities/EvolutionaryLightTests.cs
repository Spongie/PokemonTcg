using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class EvolutionaryLightTests
    {
        [TestMethod]
        public void DrawCardFromDeck()
        {
            var player = new Player();
            var pokemon = new DarkDragonair(player);
            var ability = new EvolutionaryLight(pokemon);

            player.Deck.Cards.Push(new Oddish(player));
            player.Deck.Cards.Push(new DarkGloom(player));
            player.Deck.Cards.Push(new DarkAlakazam(player));

            int oldDeckSize = player.Deck.Cards.Count;

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<Card>
            {
                player.Deck.Cards.OfType<PokemonCard>().First(x => x.Stage > 0)
            }));

            player.SetNetworkPlayer(networkPlayer);

            ability.Trigger(player, null, 0);

            Assert.AreEqual(oldDeckSize - 1, player.Deck.Cards.Count);
            Assert.AreEqual(1, player.Hand.Count);
        }
    }
}

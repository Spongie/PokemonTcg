using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.TrainerEffects;
using TCGCards.Core.Abilities;
using TCGCards.Core;
using System.Collections.ObjectModel;
using NetworkingCore;

namespace TCGCards.Tests
{
    [TestClass]
    public class PokemonCardTests
    {
        [TestMethod]
        public void DealDamage_Defender_Applied()
        {
            var game = new GameField();

            var player = new Player() { Id = NetworkId.Generate() };
            var opponent = new Player() { Id = NetworkId.Generate() };

            game.AddPlayer(player);
            game.AddPlayer(opponent);

            opponent.ActivePokemonCard = new PokemonCard()
            {
                Owner = opponent
            };

            game.ActivePlayer = opponent;
            game.NonActivePlayer = player;

            var pokemon = new PokemonCard()
            {
                Owner = player
            };

            var effect = new AttachmentEffect()
            {
                Ability = new DamageTakenModifier()
                {
                    Modifer = 20
                },
                TargetingMode = TargetingMode.YourActive
            };

            player.ActivePokemonCard = pokemon;

            effect.Process(game, player, opponent, player.ActivePokemonCard);

            pokemon.DealDamage(30, game, opponent.ActivePokemonCard, true);

            Assert.AreEqual(10, pokemon.DamageCounters);
        }
    }
}
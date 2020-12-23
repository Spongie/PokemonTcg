using Xunit;
using System.Linq;
using TCGCards.Core;
using Entities;
using TCGCards.Core.Abilities;

namespace TCGCards.Attacks.Tests
{
    public class ApplyAttackPreventionTests
    {
        [Fact]
        public void ProcessEffects_Flip_Heads()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                CoinFlip = true
            };
            pokemon.Attacks.Add(attack);

            var player = new Player()
            {
                ActivePokemonCard = pokemon
            };
            var opponent = new Player
            {
                ActivePokemonCard = new PokemonCard() { Hp = 100 }
            };

            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            game.ActivePlayer = player;
            game.NonActivePlayer = opponent;

            attack.ProcessEffects(game, player, opponent);

            Assert.Single(player.ActivePokemonCard.TemporaryAbilities.OfType<PreventStatusEffects>());
            Assert.Single(player.ActivePokemonCard.TemporaryAbilities.OfType<DamageTakenModifier>());
        }

        [Fact]
        public void ProcessEffects_Flip_Tails()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                CoinFlip = true
            };
            pokemon.Attacks.Add(attack);

            var player = new Player()
            {
                ActivePokemonCard = pokemon
            };
            var opponent = new Player
            {
                ActivePokemonCard = new PokemonCard() { Hp = 100 }
            };

            var game = new GameField().WithFlips(CoinFlipper.TAILS);
            game.ActivePlayer = player;
            game.NonActivePlayer = opponent;

            attack.ProcessEffects(game, player, opponent);

            Assert.Empty(player.ActivePokemonCard.TemporaryAbilities);
        }

        [Fact]
        public void ProcessEffects_All()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                OnlySelf = false
            };
            pokemon.Attacks.Add(attack);

            var player = new Player()
            {
                ActivePokemonCard = pokemon
            };
            var opponent = new Player
            {
                ActivePokemonCard = new PokemonCard() { Hp = 100 }
            };

            var game = new GameField();
            game.ActivePlayer = player;
            game.NonActivePlayer = opponent;

            attack.ProcessEffects(game, player, opponent);

            Assert.Single(player.ActivePokemonCard.TemporaryAbilities.OfType<PreventStatusEffects>());
            Assert.Single(player.ActivePokemonCard.TemporaryAbilities.OfType<DamageTakenModifier>());
        }
    }
}
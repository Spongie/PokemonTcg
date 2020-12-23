using Xunit;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    public class ApplyAttackFailOnTailsTests
    {
        [Fact]
        public void ProcessEffectsTest_Heads()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackFailOnTails();
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

            Assert.Single(opponent.ActivePokemonCard.TemporaryAbilities);
        }
    }
}
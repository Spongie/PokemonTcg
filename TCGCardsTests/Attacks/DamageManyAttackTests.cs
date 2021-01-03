using Xunit;
using TCGCards.Core;
using NetworkingCore;

namespace TCGCards.Attacks.Tests
{
    public class DamageManyAttackTests
    {
        [Fact]
        public void ProcessEffectsTest()
        {
            var attack = new DamageManyAttack
            {
                Damage = 20,
                EnemyBenchDamage = 10,
                SelfDamage = 30,
                TeamBenchDamage = 40
            };

            var player = new Player()
            {
                Id = NetworkId.Generate()
            };
            player.ActivePokemonCard = new PokemonCard() { Hp = 100, Owner = player };
            player.BenchedPokemon.Add(new PokemonCard() { Hp = 100, Owner = player });

            var opponent = new Player()
            {
                Id = NetworkId.Generate()
            };
            opponent.ActivePokemonCard = new PokemonCard() { Hp = 100, Owner = opponent };
            opponent.BenchedPokemon.Add(new PokemonCard() { Hp = 100, Owner = opponent });

            var game = new GameField();
            game.AddPlayer(player);
            game.AddPlayer(opponent);
            game.ActivePlayer = player;
            game.NonActivePlayer = opponent;

            attack.ProcessEffects(game, player, opponent);

            Assert.Equal(10, opponent.BenchedPokemon.GetFirst().DamageCounters);
            Assert.Equal(40, player.BenchedPokemon.GetFirst().DamageCounters);
            Assert.Equal(30, player.ActivePokemonCard.DamageCounters);
            Assert.Equal(20, attack.GetDamage(null, null, new GameField()).NormalDamage);
        }
    }
}
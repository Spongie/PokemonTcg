using Xunit;
using TCGCards.Core;

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

            var player = new Player();
            player.ActivePokemonCard = new PokemonCard() { Hp = 100 };
            player.BenchedPokemon.Add(new PokemonCard() { Hp = 100 });

            var opponent = new Player();
            opponent.ActivePokemonCard = new PokemonCard() { Hp = 100 };
            opponent.BenchedPokemon.Add(new PokemonCard() { Hp = 100 });

            attack.ProcessEffects(null, player, opponent);

            Assert.Equal(10, opponent.BenchedPokemon.GetFirst().DamageCounters);
            Assert.Equal(40, player.BenchedPokemon.GetFirst().DamageCounters);
            Assert.Equal(30, player.ActivePokemonCard.DamageCounters);
            Assert.Equal(20, attack.GetDamage(null, null, null).NormalDamage);
        }
    }
}
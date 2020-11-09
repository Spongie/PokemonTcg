using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class DamageManyAttackTests
    {
        [TestMethod()]
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

            Assert.AreEqual(10, opponent.BenchedPokemon[0].DamageCounters);
            Assert.AreEqual(40, player.BenchedPokemon[0].DamageCounters);
            Assert.AreEqual(30, player.ActivePokemonCard.DamageCounters);
            Assert.AreEqual(20, attack.GetDamage(null, null, null).NormalDamage);
        }
    }
}
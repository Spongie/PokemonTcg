using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class FrenzyTests
    {
        [TestMethod]
        public void CanActivate_Confused()
        {
            var pokemon = new DarkPrimeape(null);
            pokemon.IsConfused = true;
            var ability = (Frenzy)pokemon.Ability;

            Assert.IsTrue(ability.CanActivate());
        }

        [TestMethod]
        public void CanActivate_Not_Confused()
        {
            var pokemon = new DarkPrimeape(null);
            var ability = (Frenzy)pokemon.Ability;

            Assert.IsFalse(ability.CanActivate());
        }

        [TestMethod]
        public void Triggered_Deals_Damage()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkPrimeape(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkPrimeape(opponent);

            owner.ActivePokemonCard.IsConfused = true;
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(70, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Triggered_Deals_Damage_Self()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkPrimeape(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkPrimeape(opponent);

            owner.ActivePokemonCard.IsConfused = true;
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(60, owner.ActivePokemonCard.DamageCounters);
        }
    }
}

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TeamRocket.Attacks;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Attacks
{
    [TestClass]
    public class FrenziedAttackTests
    {
        [TestMethod]
        public void Attack_Confused()
        {
            var game = new GameField();
            game.InitTest();
            game.FirstTurn = false;

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkPrimeape(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkPrimeape(opponent);

            game.Attack((FrenziedAttack)owner.ActivePokemonCard.Attacks.First());

            Assert.IsTrue(owner.ActivePokemonCard.IsConfused);
        }
    }
}

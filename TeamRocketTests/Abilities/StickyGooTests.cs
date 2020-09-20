using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class StickyGooTests
    {
        [TestMethod]
        public void RetreatCostIncreased_0_4()
        {
            var game = new GameField();
            game.InitTest();

            var owner =game.NonActivePlayer;
                    
            var opponent = game.ActivePlayer;
            opponent.SetActivePokemon(new DarkGloom(opponent));
            opponent.BenchedPokemon.Add(new Oddish(opponent));

            game.OnActivePokemonSelected(owner.Id, new DarkMuk(owner));

            Assert.IsFalse(game.CanRetreat(opponent.ActivePokemonCard));
        }

        [TestMethod]
        public void RetreatCostIncreased_2_4()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.NonActivePlayer;

            var opponent = game.ActivePlayer;
            opponent.SetActivePokemon(new DarkGloom(opponent));
            opponent.BenchedPokemon.Add(new Oddish(opponent));

            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            owner.ActivePokemonCard = new DarkMuk(owner);

            Assert.IsFalse(game.CanRetreat(opponent.ActivePokemonCard));
        }

        [TestMethod]
        public void RetreatCostIncreased_4_4()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.NonActivePlayer;

            var opponent = game.ActivePlayer;
            opponent.SetActivePokemon(new DarkGloom(opponent));
            opponent.BenchedPokemon.Add(new Oddish(opponent));

            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            game.OnActivePokemonSelected(owner.Id, new DarkMuk(owner));

            Assert.IsTrue(game.CanRetreat(opponent.ActivePokemonCard));
        }

        [TestMethod]
        public void RetreatCostIncreased_6_4()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.NonActivePlayer;

            var opponent = game.ActivePlayer;
            opponent.SetActivePokemon(new DarkGloom(opponent));
            opponent.BenchedPokemon.Add(new Oddish(opponent));

            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            game.OnActivePokemonSelected(owner.Id, new DarkMuk(owner));

            Assert.IsTrue(game.CanRetreat(opponent.ActivePokemonCard));
        }
    }
}

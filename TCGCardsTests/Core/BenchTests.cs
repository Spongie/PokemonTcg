using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;

namespace TCGCards.Core.Tests
{
    [TestClass]
    public class BenchTests
    {
        [TestMethod]
        public void Add_Empty()
        {
            var bench = new Bench();

            bench.Add(new PokemonCard());
            bench.Add(new PokemonCard());
            bench.Add(new PokemonCard());

            Assert.AreEqual(3, bench.Count);
        }

        [TestMethod]
        public void StupidJson()
        {
            var bench = new Bench();

            var p1 = new PokemonCard();
            var p2 = new PokemonCard();
            var p3 = new PokemonCard();
            bench.Add(p1);
            bench.Add(p2);
            bench.Add(p3);

            bench.Remove(p2);

            var json = Serializer.Serialize(bench);


            Assert.Fail();
        }

        [TestMethod]
        public void Remove()
        {
            var bench = new Bench();

            var p1 = new PokemonCard();
            var p2 = new PokemonCard();
            var p3 = new PokemonCard();

            bench.Add(p1);
            bench.Add(p2);
            bench.Add(p3);

            bench.Remove(p2);

            Assert.AreEqual(2, bench.Count);
            Assert.IsTrue(bench.Contains(p1));
            Assert.IsTrue(bench.Contains(p3));
            Assert.IsFalse(bench.Contains(p2));
        }


        [TestMethod]
        public void Remove_Then_Add()
        {
            var bench = new Bench();

            var p1 = new PokemonCard();
            var p2 = new PokemonCard();
            var p3 = new PokemonCard();
            var p4 = new PokemonCard();

            bench.Add(p1);
            bench.Add(p2);
            bench.Add(p3);

            bench.Remove(p2);

            Assert.AreEqual(1, bench.GetNextFreeIndex());

            bench.Add(p4);

            Assert.AreEqual(3, bench.Count);
            Assert.IsTrue(bench.Contains(p1));
            Assert.IsFalse(bench.Contains(p2));
            Assert.IsTrue(bench.Contains(p3));
            Assert.IsTrue(bench.Contains(p4));

            Assert.AreEqual(p4, bench.Pokemons[1]);
        }

        [TestMethod]
        public void GetNextFreeIndex_Empty()
        {
            var bench = new Bench();

            Assert.AreEqual(0, bench.GetNextFreeIndex());
        }

        [TestMethod]
        public void GetNextFreeIndex_NotEmpty()
        {
            var bench = new Bench();
            bench.Add(new PokemonCard());
            bench.Add(new PokemonCard());

            Assert.AreEqual(2, bench.GetNextFreeIndex());
        }
    }
}
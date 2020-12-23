using Xunit;

namespace TCGCards.Core.Tests
{
    public class BenchTests
    {
        [Fact]
        public void Add_Empty()
        {
            var bench = new Bench();

            bench.Add(new PokemonCard());
            bench.Add(new PokemonCard());
            bench.Add(new PokemonCard());

            Assert.Equal(3, bench.Count);
        }

        [Fact]
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

            Assert.Equal(2, bench.Count);
            Assert.True(bench.Contains(p1));
            Assert.True(bench.Contains(p3));
            Assert.False(bench.Contains(p2));
        }


        [Fact]
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

            Assert.Equal(1, bench.GetNextFreeIndex());

            bench.Add(p4);

            Assert.Equal(3, bench.Count);
            Assert.True(bench.Contains(p1));
            Assert.False(bench.Contains(p2));
            Assert.True(bench.Contains(p3));
            Assert.True(bench.Contains(p4));

            Assert.Equal(p4, bench.Pokemons[1]);
        }

        [Fact]
        public void GetNextFreeIndex_Empty()
        {
            var bench = new Bench();

            Assert.Equal(0, bench.GetNextFreeIndex());
        }

        [Fact]
        public void GetNextFreeIndex_NotEmpty()
        {
            var bench = new Bench();
            bench.Add(new PokemonCard());
            bench.Add(new PokemonCard());

            Assert.Equal(2, bench.GetNextFreeIndex());
        }
    }
}
using BenchmarkDotNet.Attributes;
using NetworkingCore;
using TCGCards.Core;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class GetOpponentOfBenchmark
    {
        private readonly GameField game = new GameField();
        private Player player1;

        [GlobalSetup]
        public void Setup()
        {
            game.AddPlayer(new Player() { Id = NetworkId.Generate() });
            game.AddPlayer(new Player() { Id = NetworkId.Generate() });
            player1 = game.Players[0];
        }

        [Benchmark(Baseline = true)]
        public void Array()
        {
            game.GetOpponentOf(player1);
        }
    }
}

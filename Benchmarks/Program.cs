using BenchmarkDotNet.Running;
using Benchmarks;
using Cryptography.SimplifiedDES;

internal class Program
{
    private static void Main(string[] args)
    {
        //BenchmarkRunner.Run<RailFenceBenchmark>();
        //BenchmarkRunner.Run<KeyPhraseBenchmark>();
        //BenchmarkRunner.Run<MultiplyMethodBenchmark>();
        //BenchmarkRunner.Run<ComparisonBenchmark>();
        //BenchmarkRunner.Run<CaesarBenchmark>();
        BenchmarkRunner.Run<SDesBenchmark>();
    }
}
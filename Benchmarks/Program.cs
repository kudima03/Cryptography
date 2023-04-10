using BenchmarkDotNet.Running;
using Benchmarks;

internal class Program
{
    private static void Main(string[] args)
    {
        //BenchmarkRunner.Run<RailFenceBenchmark>();
        //BenchmarkRunner.Run<KeyPhraseBenchmark>();
        //BenchmarkRunner.Run<MultiplyMethodBenchmark>();
        //BenchmarkRunner.Run<ComparisonBenchmark>();
        //BenchmarkRunner.Run<CaesarBenchmark>();
        //BenchmarkRunner.Run<FNV1ABenchmark>();
        BenchmarkRunner.Run<RsaBenchmark>();
    }
}
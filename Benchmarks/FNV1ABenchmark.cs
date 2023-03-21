using BenchmarkDotNet.Attributes;
using Cryptography.Hashes;

namespace Benchmarks;

[MemoryDiagnoser]
public class FNV1ABenchmark
{
    private readonly string _largeText;

    public FNV1ABenchmark()
    {
        _largeText =
            File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt"));
    }

    [Benchmark]
    public void TestHash()
    {
        FNV1A.Hash(_largeText);
    }
}
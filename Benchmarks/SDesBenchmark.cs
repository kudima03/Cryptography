using BenchmarkDotNet.Attributes;
using Cryptography.SimplifiedDES;

namespace Benchmarks;

public class SDesBenchmark
{
    private readonly string _largeText;

    public SDesBenchmark()
    {
        _largeText =
            File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt"));
    }

    [Benchmark]
    public void TestSimplifiedDESEncrypt()
    {
        SimplifiedDES.Encrypt(_largeText, "1001010011");
    }

    [Benchmark]
    public void TestSimplifiedDESDecrypt()
    {
        SimplifiedDES.Encrypt(_largeText, "1001010011");
    }
}
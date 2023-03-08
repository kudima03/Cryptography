using BenchmarkDotNet.Attributes;
using Cryptography.MultiplyMethodEncryption;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class MultiplyMethodBenchmark
    {
        private readonly string _largeText;

        public MultiplyMethodBenchmark()
        {
            _largeText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt"));
        }
        [Benchmark]
        public void TestMultiplyMethodEncrypt()
        {
            MultiplyMethod.Encrypt(_largeText);
        }

        [Benchmark]
        public void TestMultiplyMethodDecrypt()
        {
            MultiplyMethod.Decrypt(_largeText, 371371);
        }
    }
}

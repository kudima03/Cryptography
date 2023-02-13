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
            _largeText = Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt");
        }
        [Benchmark]
        public void TestMultiplyMethodEncrypt()
        {
            MultiplyMethod.Encrypt("asdasdasdasdasdasdasdasdasdasdassdasdasadadasdasd");
        }

        [Benchmark]
        public void TestMultiplyMethodDecrypt()
        {
            MultiplyMethod.Decrypt("asdasdasdasdasdasdasdasdasdasdassdasdasadadasdasd", 371371);
        }
    }
}

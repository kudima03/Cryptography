using BenchmarkDotNet.Attributes;
using Cryptography.KeyPhraseEncryption;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class KeyPhraseBenchmark
    {
        private readonly string _largeText;

        public KeyPhraseBenchmark()
        {
            _largeText = Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt");
        }

        [Benchmark]
        public void TestKeyPhraseEncrypt()
        {
            KeyPhrase.Encrypt(_largeText, "qweqweqweqweqweqweqweqweqweqwe");
        }

        [Benchmark]
        public void TestKeyPhraseDecrypt()
        {
            KeyPhrase.Decrypt(_largeText, "qweqweqweqweqweqweqweqweqweqwe");
        }
    }
}

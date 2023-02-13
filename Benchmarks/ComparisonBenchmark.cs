using BenchmarkDotNet.Attributes;
using Cryptography.CaesarEncryption;
using Cryptography.KeyPhraseEncryption;
using Cryptography.MultiplyMethodEncryption;
using Cryptography.RailFenceEncryption;

namespace Benchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    public class ComparisonBenchmark
    {
        private readonly string _largeText;

        public ComparisonBenchmark()
        {
            _largeText = File.ReadAllText("C:\\Users\\Dmitry\\source\\repos\\Cryptography\\Benchmarks\\Text example.txt");
        }

        [Benchmark]
        public void KeyPhraseEncrypt()
        {
            KeyPhrase.Encrypt(_largeText, "qweqweqweqweqweqweqweqweqweqwe");
        }

        [Benchmark]
        public void KeyPhraseDecrypt()
        {
            KeyPhrase.Decrypt(_largeText, "qweqweqweqweqweqweqweqweqweqwe");
        }

        [Benchmark]
        public void MultiplyMethodEncrypt()
        {
            MultiplyMethod.Encrypt(_largeText);
        }

        [Benchmark]
        public void MultiplyMethodDecrypt()
        {
            MultiplyMethod.Decrypt(_largeText, 371371);
        }

        [Benchmark]
        public void CaesarEncryptTest()
        {
            Caesar.Encrypt(_largeText, 15);
        }

        [Benchmark]
        public void CaesarDecryptTest()
        {
            Caesar.Decrypt(_largeText, 15);
        }
        [Benchmark]
        public void RailFenceEncrypt()
        {
            RailFence.Encrypt(_largeText, 50);
        }

        [Benchmark]
        public void RailFenceDecrypt()
        {
            RailFence.Decrypt(_largeText, 50);
        }
    }
}

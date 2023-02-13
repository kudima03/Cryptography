using BenchmarkDotNet.Attributes;
using Cryptography.KeyPhraseEncryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class KeyPhraseBenchmark
    {
        private readonly string _largeText;

        public KeyPhraseBenchmark()
        {
            _largeText = File.ReadAllText("C:\\Users\\Dmitry\\source\\repos\\Cryptography\\Benchmarks\\Text example.txt");
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

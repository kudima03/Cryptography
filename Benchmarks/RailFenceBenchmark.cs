using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography.RailFenceEncryption;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class RailFenceBenchmark
    {
        private readonly string _largeText;

        private readonly int _key;

        public RailFenceBenchmark()
        {
            _key = 2;
            _largeText = Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt");
        }
        [Benchmark]
        public void TestRailFenceEncrypt()
        {
            RailFence.Encrypt(_largeText, _key);
        }

        [Benchmark]
        public void TestRailFenceDecrypt()
        {
            RailFence.Decrypt(_largeText, _key);
        }
    }
}

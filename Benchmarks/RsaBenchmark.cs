using BenchmarkDotNet.Attributes;
using Cryptography.MultiplyMethodEncryption;
using Cryptography.RSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Benchmarks
{
    [MemoryDiagnoser]
    public class RsaBenchmark
    {
        private readonly string _largeText;

        private (UInt128[] message, UInt128 key, UInt128 r) data;

        public RsaBenchmark()
        {
            _largeText =
                File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt"));
        }

        [Benchmark]
        public void TestRsaMethodEncrypt()
        {
            data = RSA.Encrypt(_largeText);      
        }

        [Benchmark]
        public void TestRsaMethodDecrypt()
        {
            RSA.Decrypt(data.message, data.key, data.r);
        }
    }
}

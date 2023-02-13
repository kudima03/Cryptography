﻿using BenchmarkDotNet.Attributes;
using Cryptography.CaesarEncryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class CaesarBenchmark
    {
        private readonly string _largeText;
        public CaesarBenchmark()
        {
            _largeText = Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt");
        }

        [Benchmark]
        public void EncryptTest()
        {
            Caesar.Encrypt(_largeText, 15);
        }

        [Benchmark]
        public void DecryptTest()
        {
            Caesar.Decrypt(_largeText, 15);
        }
    }
}

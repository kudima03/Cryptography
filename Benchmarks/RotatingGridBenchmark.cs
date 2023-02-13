using BenchmarkDotNet.Attributes;
using Cryptography.RotatingGridEncryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class RotatingGridBenchmark
    {
        private readonly string text = "Dimmito mihe quo";

        private readonly char[][] _keyMatrixEncrypt;
        private readonly char[][] _keyMatrixDecrypt;

        public RotatingGridBenchmark()
        {
            _keyMatrixEncrypt = new char[][]
            {
                new char[] { '1', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', '2'},
                new char[] { ' ', ' ', '4', ' '},
                new char[] { ' ', '3', ' ', ' '},
            };

            _keyMatrixDecrypt = new char[][]
            {
                new char[] { '1', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', '2'},
                new char[] { ' ', ' ', '4', ' '},
                new char[] { ' ', '3', ' ', ' '},
            };
        }

        [Benchmark]
        public void EncryptBenchmark()
        {
            RotatingGrid.Encrypt(text, _keyMatrixEncrypt);
        }

        [Benchmark]
        public void DecryptBenchmark()
        {
            RotatingGrid.Decrypt(text, _keyMatrixDecrypt);
        }
    }
}

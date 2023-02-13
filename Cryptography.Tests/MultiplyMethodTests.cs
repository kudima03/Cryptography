using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography.MultiplyMethodEncryption;

namespace Cryptography.Tests
{
    [TestClass]
    public class MultiplyMethodTests
    {
        private readonly string _largeTextExample;
        public MultiplyMethodTests()
        {
            _largeTextExample = File.ReadAllText("C:\\Users\\Dmitry\\source\\repos\\Cryptography\\Benchmarks\\Text example.txt");
        }

        [TestMethod]
        public void TestEncryptDecryptEquals()
        {
            var encryptedStr = MultiplyMethod.Encrypt(_largeTextExample);
            Assert.AreEqual(MultiplyMethod.Decrypt(encryptedStr.encryptedText, encryptedStr.decryptKey), _largeTextExample);
        }

        [TestMethod]
        public void TestEncryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MultiplyMethod.Encrypt(null));
        }

        [TestMethod]
        public void TestDecryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MultiplyMethod.Decrypt(null, 15));
        }

        [TestMethod]
        public void TestDecryptLessZeroKeyParameter()
        {
            Assert.ThrowsException<ArgumentException>(() => MultiplyMethod.Decrypt(_largeTextExample, -100));
        }
    }
}

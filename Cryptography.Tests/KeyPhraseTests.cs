using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography.KeyPhraseEncryption;

namespace Cryptography.Tests
{
    [TestClass]
    public class KeyPhraseTests
    {
        private readonly string _largeTextExample;
        public KeyPhraseTests()
        {
            _largeTextExample = File.ReadAllText("C:\\Users\\Dmitry\\source\\repos\\Cryptography\\Benchmarks\\Text example.txt");
        }

        [TestMethod]
        public void TestEncryptDecryptEquals()
        {
            var keyPhrase = "qweqweqweqweqweqkjdfsdfsjdfksldbjfowierbfjihgidbfnkjewfdghifjjewipdbjfjiewoweqw";
            var encryptedStr = KeyPhrase.Encrypt(_largeTextExample, keyPhrase);
            Assert.AreEqual(KeyPhrase.Decrypt(encryptedStr, keyPhrase), _largeTextExample);
        }

        [TestMethod]
        public void TestEncryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => KeyPhrase.Encrypt(null, null));
        }

        [TestMethod]
        public void TestDecryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => KeyPhrase.Decrypt(null, null));
        }
    }
}

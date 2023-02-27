using Cryptography.RailFenceEncryption;
using NUnit.Framework.Internal;

namespace Cryptography.Tests
{
    [TestClass]
    public class RailFenceTests
    {
        private readonly string _largeTextExample;
        public RailFenceTests()
        {
            _largeTextExample = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt"));
        }

        [TestMethod]
        public void TestEncryptDecryptEquals()
        {
            var encryptedStr = RailFence.Encrypt(_largeTextExample, 5);
            Assert.AreEqual(RailFence.Decrypt(encryptedStr, 5), _largeTextExample);
        }

        [TestMethod]
        public void TestEncryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(()=>RailFence.Encrypt(null, 5));
        }

        [TestMethod]
        public void TestDecryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RailFence.Decrypt(null, 5));
        }

        [TestMethod]
        public void TestDecryptKeyZeroParameter()
        {
            Assert.ThrowsException<ArgumentException>(() => RailFence.Decrypt(_largeTextExample, 0));
        }
        [TestMethod]
        public void TestEncryptKeyZeroParameter()
        {
            Assert.ThrowsException<ArgumentException>(() => RailFence.Encrypt(_largeTextExample, 0));
        }
        [TestMethod]
        public void TestDecryptKeyLessZeroParameter()
        {
            Assert.ThrowsException<ArgumentException>(() => RailFence.Decrypt(_largeTextExample, -10));
        }
        [TestMethod]
        public void TestEncryptKeyLessZeroParameter()
        {
            Assert.ThrowsException<ArgumentException>(() => RailFence.Encrypt(_largeTextExample, -10));
        }
    }
}
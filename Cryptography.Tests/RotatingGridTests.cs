using Cryptography.RotatingGridEncryption;

namespace Cryptography.Tests
{
    [TestClass]
    public class RotatingGridTests
    {
        [TestMethod]
        public void TestEcryptDecryptMatrix3()
        {
            var testText = "Dimmito mihe quo";

            var encryptedText = RotatingGrid.Encrypt(testText);

            var decryptedText = RotatingGrid.Decrypt(encryptedText.encryptedStr, encryptedText.grid);

            Assert.AreEqual(testText, decryptedText);
        }

        [TestMethod]
        public void TestEcryptDecryptMatrix5()
        {
            var testText = "Dimmito mihe quoniam ego";

            var encryptedText = RotatingGrid.Encrypt(testText);

            var decryptedText = RotatingGrid.Decrypt(encryptedText.encryptedStr, encryptedText.grid);

            Assert.AreEqual(testText, decryptedText);
        }

        [TestMethod]
        public void TestEcryptDecryptMatrix6()
        {
            var testText = "Dimmito mihe quoniam ego sum optimus";

            var encryptedText = RotatingGrid.Encrypt(testText);

            var decryptedText = RotatingGrid.Decrypt(encryptedText.encryptedStr, encryptedText.grid);

            Assert.AreEqual(testText, decryptedText);
        }

        [TestMethod]
        public void TestEncryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RotatingGrid.Encrypt(null));
        }

        [TestMethod]
        public void TestDecryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RotatingGrid.Decrypt(null, null));
        }
    }
}

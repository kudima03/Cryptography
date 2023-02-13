using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var matrix = new char[][]
            {
                new char[] { '1', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', '2'},
                new char[] { ' ', ' ', '4', ' '},
                new char[] { ' ', '3', ' ', ' '},
            };

            var matrixCopy = new char[][]
            {
                new char[] { '1', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', '2'},
                new char[] { ' ', ' ', '4', ' '},
                new char[] { ' ', '3', ' ', ' '},
            };

            var encryptedText = RotatingGrid.Encrypt(testText, matrix);

            var decryptedText = RotatingGrid.Decrypt(encryptedText, matrixCopy);

            Assert.AreEqual(testText, decryptedText);
        }

        [TestMethod]
        public void TestEcryptDecryptMatrix5()
        {
            var testText = "Dimmito mihe quoniam ego ";

            var matrix = new char[][]
            {
                new char[] { '1', '2', '3', '4', ' '},
                new char[] { ' ', ' ', '6', '5', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' '},
            };

            var matrixCopy = new char[][]
            {
                new char[] { '1', '2', '3', '4', ' '},
                new char[] { ' ', ' ', '6', '5', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' '},
            };

            var encryptedText = RotatingGrid.Encrypt(testText, matrix);

            var decryptedText = RotatingGrid.Decrypt(encryptedText, matrixCopy);

            Assert.AreEqual(testText, decryptedText);
        }

        [TestMethod]
        public void TestEcryptDecryptMatrix6()
        {
            var testText = "Dimmito mihe quoniam ego sum optimus";

            var matrix = new char[][]
            {
                new char[] { '1', '2', '3', '4', '5', ' '},
                new char[] { ' ', ' ', '7', '8', '6', ' '},
                new char[] { ' ', ' ', ' ', '9', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
            };

            var matrixCopy = new char[][]
            {
                new char[] { '1', '2', '3', '4', '5', ' '},
                new char[] { ' ', ' ', '7', '8', '6', ' '},
                new char[] { ' ', ' ', ' ', '9', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
            };

            var encryptedText = RotatingGrid.Encrypt(testText, matrix);

            var decryptedText = RotatingGrid.Decrypt(encryptedText, matrixCopy);

            Assert.AreEqual(testText, decryptedText);
        }

        [TestMethod]
        public void TestEncryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RotatingGrid.Encrypt(null, null));
        }

        [TestMethod]
        public void TestDecryptNullStringParameter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RotatingGrid.Decrypt(null, null));
        }

        [TestMethod]
        public void TestInvalidGridParameter()
        {
            var matrix = new char[][]
            {
                new char[] { '1', ' ', ' ', ' '},
                new char[] { ' ', ' ', ' ', '2'},
                new char[] { ' ', '5', '4', ' '},
                new char[] { ' ', '3', ' ', ' '},
            };
            Assert.ThrowsException<ArgumentException>(() => RotatingGrid.Decrypt("asdsadasd", matrix));
        }
    }
}

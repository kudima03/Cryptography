using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography.Hashes;

namespace Cryptography.DigitalSignature
{
    internal class RsaSignature
    {
        public (UInt128[] signValue, UInt128 openKey, UInt128 r) GenerateSign(string document)
        {
            var hashedDocument = FNV1A.Hash(document);
            var encryptionResult = RSA.RSA.Encrypt(hashedDocument.ToString());
            return (encryptionResult.encryptedText, encryptionResult.key, encryptionResult.r);
        }

        public bool ValidateSign(string document, UInt128[] sign, UInt128 e, UInt128 r)
        {
            var hashedDocument = FNV1A.Hash(document);
            var decryptionResult = RSA.RSA.Decrypt(sign, e, r);
            return hashedDocument.ToString() == decryptionResult;
        }
    }
}

using Cryptography.KeyPhraseEncryption;
using Cryptography.MultiplyMethodEncryption;
using Cryptography.RailFenceEncryption;
using Cryptography.RotatingGridEncryption;
using Cryptography.SimplifiedDES;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        SimplifiedDES.Encrypt("asdasd", Encoding.Unicode, "1001010011");

    }
}
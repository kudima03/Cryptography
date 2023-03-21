using Cryptography.CaesarEncryption;
using Cryptography.KeyPhraseEncryption;
using Cryptography.MultiplyMethodEncryption;
using Cryptography.RailFenceEncryption;
using Cryptography.RotatingGridEncryption;
using Cryptography.SimplifiedDES;
using System.Text;
using Cryptography.RSA;

internal class Program
{
    private static void Main(string[] args)
    {
        var q = new[]
        {
            (char)6,
            (char)4,
            (char)10
        };

        var n = /*"123";*/ new string(q);

        var a = RSA.Encrypt(n);
        //Console.WriteLine(n);
        var b = RSA.Decrypt(a.encryptedText, a.key, a.r);
        Console.WriteLine(b == n);

/*        RailFence.Demo();

        Console.WriteLine();
        Console.WriteLine();

        KeyPhrase.Demo();

        Console.WriteLine();
        Console.WriteLine();

        RotatingGrid.Demo();

        Console.WriteLine();
        Console.WriteLine();

        Caesar.Demo();

        Console.WriteLine();
        Console.WriteLine();

        MultiplyMethod.Demo();*/

    }
}
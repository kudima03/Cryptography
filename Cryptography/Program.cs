using Cryptography.CaesarEncryption;
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

        var hash = Cryptography.Hashes.FNV1A.Hash("BSUIR");

        Console.WriteLine(hash);

        var a = 4286365170;

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
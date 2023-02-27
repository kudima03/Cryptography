using Cryptography.CaesarEncryption;
using Cryptography.KeyPhraseEncryption;
using Cryptography.MultiplyMethodEncryption;
using Cryptography.RailFenceEncryption;
using Cryptography.RotatingGridEncryption;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        RailFence.Demo();

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

        MultiplyMethod.Demo();

    }
}
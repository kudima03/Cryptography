using Cryptography.RSA;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var str = Console.ReadLine();

        var timer = new Stopwatch();
        timer.Start();

        int success = 0;

        for (int i = 0; i < 100; i++)
        {
            var a = RSA.Encrypt(str);

            var b = RSA.Decrypt(a.encryptedText, a.key, a.r);

            /*            Console.WriteLine(b == str);
                        Console.WriteLine( "===================================================");*/

            if (b == str)
            {
                success++;
            }
            Console.Clear();
            Console.WriteLine(i + 1 + "%");
        }
        timer.Stop();
        Console.WriteLine("Success: " + success + "%");
        Console.WriteLine(timer.ElapsedMilliseconds);
    }
}
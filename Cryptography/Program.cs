using Cryptography.RSA;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        //File.ReadAllText("C:\\Users\\Dmitry\\source\\repos\\Cryptography\\Benchmarks\\Text examples\\Text example.txt")
        
        var str = new string('q', 100);

        var timer = new Stopwatch();

        timer.Start();

        var encrypted = RSA.Encrypt(str);

        var decrypted = RSA.Decrypt(encrypted.encryptedText, encrypted.key, encrypted.r);

        timer.Stop();

        Console.WriteLine(timer.ElapsedMilliseconds);
    }
}
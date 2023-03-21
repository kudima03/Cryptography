using System.Numerics;
using System.Text;

namespace Cryptography.RSA;

public class RSA
{
    private static readonly uint[] _primes;

    private static readonly Random rand;

    static RSA()
    {
        _primes = GetPrimesFromFile();
        rand = new Random();
    }

    private static uint[] GetPrimesFromFile()
    {
        var primes = new List<uint>();

        /*            new OptimizedSegmentedWheel235(1000).ListPrimes(primes.Add);

                    primes = primes.Skip((int)0.9 * primes.Count).ToList();

                    using (var file = new StreamWriter(File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), "Primes.txt"))))
                    {
                        foreach (var item in primes)
                        {
                            file.WriteLine(item);
                        }
                    }*/

        using (var file = new StreamReader(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "RSA",
                   "Primes.txt"))))
        {
            while (!file.EndOfStream) primes.Add(uint.Parse(file.ReadLine()));
        }

        return primes.ToArray();
        //return null;
    }

    //Extended Euclid algorithm:  x*a + y*b = d, where d = GCD(a, b)
    public static (ulong x, ulong y, ulong gcd) EuclidExtendedAlgorithhm(ulong a, ulong b)
    {
        if (a == 0) return (0, 1, b);

        var results = EuclidExtendedAlgorithhm(b % a, a);

        return (results.y - b / a * results.x, results.x, results.gcd);
    }

    private static ulong EulerFunctionForCompositeNumber(ulong a, ulong b)
    {
        return (a - 1) * (b - 1);
    }

    private static ulong[] GetRandomPrimes(ulong maxValue, int amount)
    {
        var selectedPrimes = new ulong[amount];

        for (var i = 0; i < amount; i++)
        {
            var index = rand.Next(_primes.Count());
            while (_primes[index] >= maxValue) index = rand.Next(_primes.Count());

            selectedPrimes[i] = _primes[index];
        }

        return selectedPrimes;
    }

    private static BigInteger Power(ulong x, ulong n)
    {
        checked
        {
            BigInteger result = 1;
            BigInteger bufX = x;
            while (n > 0)
                if ((n & 1) == 0)
                {
                    bufX *= bufX;
                    n >>= 1;
                }
                else
                {
                    result *= bufX;
                    --n;
                }

            return result;
        }
    }

    private static string Encode(string text, ulong key, ulong r)
    {
        var sb = new StringBuilder(text.Length);

        for (var i = 0; i < text.Length; i++)
        {
            var s = Convert.ToUInt64(text[i]);
            var x = Power(s, key);
            var a = (ulong)(x % r);
            var m = Convert.ToChar(a);
            sb.Append(m);
        }

        /*            Parallel.For(0, text.Length, (i) =>
                    {

                    });*/
        return sb.ToString();
    }

    private static ulong ModInversion(ulong value, ulong modulo)
    {
        var egcd = EuclidExtendedAlgorithhm(value, modulo);

        if (egcd.gcd != 1)
            throw new ArgumentException("Invalid modulo", nameof(modulo));

        var result = egcd.x;

        if (result < 0)
            result += modulo;

        return result % modulo;
    }

    public static (string encryptedText, ulong key, ulong r) Encrypt(string text)
    {
        var primes = new ulong[] { 17, 31 }; /*GetRandomPrimes(ulong.MaxValue, 2);*/

        var r = primes[0] * primes[1];

        //f(r)
        var eulerValue = EulerFunctionForCompositeNumber(primes[0], primes[1]);

        primes = /*new ulong[] { 7 };*/ GetRandomPrimes(eulerValue, 1);

        //e
        var e = primes[0];

        var d = ModInversion(e, eulerValue);

        return (Encode(text, e, r), d, r);
    }

    public static string Decrypt(string text, ulong key, ulong r)
    {
        return Encode(text, key, r);
    }
}
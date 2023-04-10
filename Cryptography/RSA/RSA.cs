using System.Numerics;
using System.Text;

namespace Cryptography.RSA;

public class RSA
{
    private static readonly UInt128[] _primes;

    private static readonly Random rand;

    static RSA()
    {
        _primes = GetPrimesFromFile();
        rand = new Random();
    }

    private static UInt128[] GetPrimesFromFile()
    {
        var primes = new List<UInt128>();

        if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Primes.txt")))
        {
            using (var writer = new StreamWriter(File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), "Primes.txt"))))
            {
                new PrimesGenerator(300).Generate(writer);
            }
        }

        using (var file = new StreamReader(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Primes.txt"))))
        {
            while (!file.EndOfStream) primes.Add(UInt128.Parse(file.ReadLine()));
        }

        return primes.ToArray();
    }

    //Extended Euclid algorithm:  x*a + y*b = d, where d = GCD(a, b)
    public static (BigInteger x, BigInteger y, BigInteger gcd) EuclidExtendedAlgorithhm(BigInteger a, BigInteger b)
    {
        if (a == 0) return (0, 1, b);

        var results = EuclidExtendedAlgorithhm(b % a, a);

        return (results.y - b / a * results.x, results.x, results.gcd);
    }

    private static UInt128 EulerFunctionForCompositeNumber(UInt128 a, UInt128 b)
    {
        return (a - 1) * (b - 1);
    }

    private static UInt128[] GetRandomPrimes(UInt128 maxValue, int amount)
    {
        var selectedPrimes = new UInt128[amount];

        for (var i = 0; i < amount; i++)
        {
            var index = rand.Next(_primes.Count());
            while (_primes[index] >= maxValue
                || selectedPrimes.Contains(_primes[index]))
            {
                index = rand.Next(_primes.Count());
            }

            selectedPrimes[i] = _primes[index];
        }

        return selectedPrimes;
    }

    private static BigInteger Power(UInt128 x, UInt128 n)
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

    private static UInt128 ModInversion(UInt128 value, UInt128 modulo)
    {
        var res = EuclidExtendedAlgorithhm(value, modulo);
        return res.gcd > 1 ? 0 : (UInt128)(res.x % modulo + modulo) % modulo;
    }

    private static (UInt128 e, UInt128 d, UInt128 r) GetKeys()
    {
        var primes = GetRandomPrimes(ulong.MaxValue, 2);

        var r = primes[0] * primes[1];

        //f(r)
        var eulerValue = EulerFunctionForCompositeNumber(primes[0], primes[1]);

        primes = GetRandomPrimes(eulerValue, 1);

        //e
        var e = primes[0];

        var d = ModInversion(e, eulerValue);

        return (e, d, r);
    }

    private static UInt128[] Encode(UInt128[] text, UInt128 key, UInt128 r)
    {
        Parallel.For(0, text.Length, (i) =>
        {
            text[i] = (UInt128)(Power(text[i], key) % r);
        });

        return text;
    }

    public static (UInt128[] encryptedText, UInt128 key, UInt128 r) Encrypt(string text)
    {
        var keys = GetKeys();

        var encryptedStr = Encode(text.Select(x => (UInt128)x + 2).ToArray(), keys.e, keys.r);

        return (encryptedStr, keys.d, keys.r);
    }

    public static string Decrypt(UInt128[] text, UInt128 key, UInt128 r)
    {
        var sb = new StringBuilder(text.Length);
        sb.Append(Encode(text, key, r).Select(x => (char)(x - 2)).ToArray());
        return sb.ToString();
    }
}
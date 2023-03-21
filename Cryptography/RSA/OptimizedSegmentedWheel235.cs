namespace Cryptography.RSA;

public class OptimizedSegmentedWheel235
{
    private const int BUFFER_LENGTH = 200 * 1024;
    private const int WHEEL = 30;
    private const int WHEEL_PRIMES_COUNT = 3;

    private static readonly long[] WheelRemainders = { 1, 7, 11, 13, 17, 19, 23, 29 };
    private static readonly long[] SkipPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
    private static readonly byte[] Masks = { 1, 2, 4, 8, 16, 32, 64, 128 };
    private static readonly int[][] OffsetsPerByte;
    private readonly long[] FirstPrimes;

    private readonly long Length;
    private readonly long[][] PrimeMultiples;

    static OptimizedSegmentedWheel235()
    {
        OffsetsPerByte = new int[256][];
        var offsets = new List<int>();
        for (var b = 0; b < 256; b++)
        {
            offsets.Clear();
            for (var i = 0; i < WheelRemainders.Length; i++)
                if ((b & Masks[i]) != 0)
                    offsets.Add((int)WheelRemainders[i]);
            OffsetsPerByte[b] = offsets.ToArray();
        }
    }

    public OptimizedSegmentedWheel235(long length)
    {
        Length = length;
        var firstChunkLength = (int)Math.Sqrt(length) + 1;
        var sieve = new SieveOfEratosthenes(firstChunkLength);
        var firstPrimes = new List<long>();
        sieve.ListPrimes(firstPrimes.Add);
        FirstPrimes = firstPrimes.Skip(WHEEL_PRIMES_COUNT).ToArray();
        PrimeMultiples = new long[WheelRemainders.Length][];
        for (var i = 0; i < WheelRemainders.Length; i++)
        {
            PrimeMultiples[i] = new long[FirstPrimes.Length];
            for (var j = 0; j < FirstPrimes.Length; j++)
            {
                var prime = FirstPrimes[j];
                var val = prime * prime;
                while (val % WHEEL != WheelRemainders[i]) val += 2 * prime;
                PrimeMultiples[i][j] = (val - WheelRemainders[i]) / WHEEL;
            }
        }
    }

    private void SieveSegment(byte[] segmentData, long segmentStart, long segmentEnd)
    {
        for (var i = 0; i < segmentData.Length; i++) segmentData[i] = 255;
        var segmentLength = segmentEnd - segmentStart;

        for (var i = 0; i < WheelRemainders.Length; i++)
        {
            var mask = (byte)~Masks[i];
            for (var j = 0; j < PrimeMultiples[i].Length; j++)
            {
                var current = PrimeMultiples[i][j] - segmentStart;
                if (current >= segmentLength) continue;
                var prime = FirstPrimes[j];

                while (current < segmentLength)
                {
                    segmentData[current] &= mask;
                    current += prime;
                }

                PrimeMultiples[i][j] = segmentStart + current;
            }
        }
    }

    public void ListPrimes(Action<long> callback)
    {
        foreach (var prime in SkipPrimes)
            if (prime < Length)
                callback.Invoke(prime);

        var max = (Length + WHEEL - 1) / WHEEL;
        var segmentData = new byte[BUFFER_LENGTH];
        long segmentStart = 1;
        var segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
        while (segmentStart < max)
        {
            SieveSegment(segmentData, segmentStart, segmentEnd);
            for (var i = 0; i < segmentData.Length; i++)
            {
                var offset = (segmentStart + i) * WHEEL;
                var data = segmentData[i];
                var offsets = OffsetsPerByte[data];
                for (var j = 0; j < offsets.Length; j++)
                {
                    var p = offset + offsets[j];
                    if (p >= Length) break;
                    callback.Invoke(p);
                }
            }

            segmentStart = segmentEnd;
            segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
        }
    }
}
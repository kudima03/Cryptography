using System.Collections;

namespace Cryptography.RSA;

public class SieveOfEratosthenes
{
    private readonly BitArray Data;

    public SieveOfEratosthenes(int length)
    {
        Data = new BitArray(length);
        Data.SetAll(true);

        for (var p = 2; p * p < length; p++)
            if (Data[p])
                for (var i = p * p; i < Length; i += p)
                    Data[i] = false;
    }

    public int Length => Data.Length;

    public void ListPrimes(Action<long> callback)
    {
        for (var i = 2; i < Length; i++)
            if (Data[i])
                callback.Invoke(i);
    }
}
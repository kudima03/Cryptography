using System.Collections;
using System.ComponentModel;

namespace Cryptography.RSA
{
    public class SieveOfEratosthenes
    {
        private BitArray Data;
        public int Length => Data.Length;

        public SieveOfEratosthenes(int length)
        {
            Data = new BitArray(length);
            Data.SetAll(true);

            for (int p = 2; p * p < length; p++)
            {
                if (Data[p])
                {
                    for (int i = p * p; i < Length; i += p)
                    {
                        Data[i] = false;
                    }
                }
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            for (int i = 2; i < Length; i++)
            {
                if (Data[i]) callback.Invoke(i);
            }
        }
    }
}
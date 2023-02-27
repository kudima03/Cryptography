using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography.SimplifiedDES
{
    internal class SimplifiedDES
    {
        private static readonly int[] P10_KEY_MIX_TEMPLATE = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };

        private static readonly int[] P8_KEY_MIX_TEMPLATE = { 6, 3, 7, 4, 8, 5, 10, 9 };

        private static readonly int[] P8_TEXT_MIX_TEMPLATE = { 2, 6, 3, 1, 4, 8, 5, 7 };

        private static readonly int[] P8_TEXT_MIX_TEMPLATE_FINAL = { 4, 1, 3, 5, 7, 2, 8, 6 };

        private static void Mix(ref BitArray bitArray, int[] mixTemplate)
        {
            var newBitArray = new BitArray(mixTemplate.Length);

            for (int i = 0; i < newBitArray.Length; i++)
            {
                newBitArray[i] = bitArray[mixTemplate[i] - 1];
            }

            bitArray = newBitArray;
        }

        private static void CyclicLeftShift(BitArray bitArray, int bitsAmount)
        {
            var bitsToShift = new bool[bitsAmount];
            var counter = 0;

            int i = 0;

            for (int k = i; k < i + bitsAmount; k++)
            {
                bitsToShift[counter++] = bitArray[k];
            }

            while (i < (bitArray.Length / 2) - bitsAmount)
            {
                bitArray[i] = bitArray[i++ + bitsAmount];
            }

            counter = 0;

            for (int k = (bitArray.Length / 2) - bitsAmount; k < (bitArray.Length / 2); k++)
            {
                bitArray[k] = bitsToShift[counter++];
            }

            i += bitsAmount;

            var boolArray1 = new bool[bitArray.Length];

            bitArray.CopyTo(boolArray1, 0);

            counter = 0;

            for (int k = i; k < i + bitsAmount; k++)
            {
                bitsToShift[counter++] = bitArray[k];
            }

            while (i < bitArray.Length - bitsAmount)
            {
                bitArray[i] = bitArray[i++ + bitsAmount];
            }

            counter = 0;

            for (int k = bitArray.Length - bitsAmount; k < bitArray.Length; k++)
            {
                bitArray[k] = bitsToShift[counter++];
            }
        }

        private static void Swap(BitArray bitArray)
        {
            var boolArray = new bool[bitArray.Length];
            bitArray.CopyTo(boolArray, 0);
            bitArray = bitArray.LeftShift(bitArray.Length / 2);

            var counter = boolArray.Length / 2;
            for (int i = 0; i < bitArray.Length / 2; i++)
            {
                bitArray[i] = boolArray[counter++];
            }
        }

        private static void CheckKey(BitArray key)
        {
            int counter = 0;
            foreach (var item in key)
            {
                if ((bool)item == true)
                {
                    Console.Write(1);
                }
                else
                {
                    Console.Write(0);
                }
                counter++;
                if (counter == 8)
                {
                    counter = 0;
                    Console.Write(' ');
                }
            }
            Console.WriteLine("\n");
        }

        private static (BitArray roundKey1, BitArray roundKey2) GenerateRoundKeys(BitArray key)
        {
            Mix(ref key, P10_KEY_MIX_TEMPLATE);
            var secondKey = (BitArray)key.Clone();
            CyclicLeftShift(key, 1);
            CyclicLeftShift(secondKey, 3);
            Mix(ref key, P8_KEY_MIX_TEMPLATE);
            Mix(ref secondKey, P8_KEY_MIX_TEMPLATE);
            return (key, secondKey);
        }

        private static string Encrypt(byte[] text, BitArray key)
        {
            var keys = GenerateRoundKeys(key);

            foreach (var item in text)
            {
                var bitArray = new BitArray(new byte[] { text[0] });
                Mix(ref bitArray, P8_TEXT_MIX_TEMPLATE);
                bitArray = bitArray.Xor(keys.roundKey1);
                Swap(bitArray);
                bitArray = bitArray.Xor(keys.roundKey2);
                Mix(ref bitArray, P8_TEXT_MIX_TEMPLATE);
            }

            return "";
        }

        public static string Encrypt(string text, Encoding encoding, string key)
        {
            if (key.Length != 10) throw new ArgumentException("Key length must be 10");
            var bitKey = new BitArray(10);
            for (int i = 0; i < 10; i++)
            {
                switch (key[i])
                {
                    case '1':
                        {
                            bitKey[i] = true;
                            break;
                        }
                    case '0':
                        {
                            bitKey[i] = false;
                            break;
                        }
                    default:
                        throw new ArgumentException("Key must contain only 1 or 0");
                }
            }
            return Encrypt(encoding.GetBytes(text), bitKey);
        }
    }
}

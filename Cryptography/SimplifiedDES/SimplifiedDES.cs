using System.Collections;
using System.Text;

namespace Cryptography.SimplifiedDES
{
    public static class SimplifiedDES
    {
        private static readonly int[] P10_KEY_MIX_TEMPLATE = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };

        private static readonly int[] P8_KEY_MIX_TEMPLATE = { 6, 3, 7, 4, 8, 5, 10, 9 };

        private static readonly int[] P8_TEXT_MIX_TEMPLATE = { 2, 6, 3, 1, 4, 8, 5, 7 };

        private static readonly int[] P8_TEXT_MIX_TEMPLATE_FINAL = { 4, 1, 3, 5, 7, 2, 8, 6 };

        private static readonly int[] P8_EXPANSION_RULE = { 4, 1, 2, 3, 2, 3, 4, 1 };

        private static readonly int[] P4_MIX_RULE = { 2, 4, 3, 1 };

        private static readonly BitArray[,] S_BLOCK1 = new BitArray[4, 4];

        private static readonly BitArray[,] S_BLOCK2 = new BitArray[4, 4];

        static SimplifiedDES()
        {
            int[,] s_block1_integer =
            {
                { 1, 0, 3, 2},
                { 3, 2, 1, 0},
                { 0, 2, 1, 3},
                { 3, 1, 3, 2}
            };

            int[,] s_block2_integer =
            {
                { 0, 1, 2, 3},
                { 2, 0, 1, 3},
                { 3, 0, 1, 0},
                { 2, 1, 0, 3}
            };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    S_BLOCK1[i, j] = Int32ToBitArray(s_block1_integer[i, j]);
                    S_BLOCK2[i, j] = Int32ToBitArray(s_block2_integer[i, j]);
                }
            }
        }

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

        private static int BitArrayToInt32(BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int digit = 0;

            int result = 0;

            for (int i = bitArray.Length - 1; i >= 0; i--)
            {
                result += (bitArray[i] ? 1 : 0) * (int)Math.Pow(2, digit++);
            }
            return result;
        }

        private static BitArray Int32ToBitArray(int value)
        {
            var bitArray = new BitArray(2);
            for (int i = 1; i >= 0; i--)
            {
                if ((value & 1) != 0)
                    bitArray[i] = true;
                else
                    bitArray[i] = false;
                value >>= 1;
            }
            return bitArray;
        }

        private static BitArray StringToBitArray(string str)
        {
            var bitKey = new BitArray(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
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
            return bitKey;
        }

        private static BitArray GetSequenceFromSBlocks(BitArray bitArray)
        {
            var bitBuf = new BitArray(2);
            bitBuf[0] = bitArray[0];
            bitBuf[1] = bitArray[3];

            var row = BitArrayToInt32(bitBuf);

            bitBuf[0] = bitArray[1];
            bitBuf[1] = bitArray[2];
            var column = BitArrayToInt32(bitBuf);

            var value = S_BLOCK1[row, column];

            var sequence = new BitArray(4);

            sequence[0] = value[0];
            sequence[1] = value[1];

            row = BitArrayToInt32(bitBuf);

            bitBuf[0] = bitArray[5];
            bitBuf[1] = bitArray[6];
            column = BitArrayToInt32(bitBuf);

            value = S_BLOCK2[row, column];

            sequence[2] = value[0];
            sequence[3] = value[1];
            return sequence;
        }

        private static void Round(BitArray input, BitArray key)
        {
            var leftHalf = new BitArray(input.Count / 2);
            var rightHalf = new BitArray(input.Count / 2);
            for (int i = 0; i < input.Count / 2; i++)
            {
                leftHalf[i] = input[i];
                rightHalf[i] = input[i + input.Count / 2];
            }

            Mix(ref rightHalf, P8_EXPANSION_RULE);

            rightHalf.Xor(key);

            var sequence = GetSequenceFromSBlocks(rightHalf);

            Mix(ref sequence, P4_MIX_RULE);

            sequence.Xor(leftHalf);

            for (int i = 0; i < sequence.Length; i++)
            {
                input[i] = sequence[i];
            }
        }

        private static byte[] Chipher(byte[] text, BitArray key, bool encrypt)
        {
            var keys = GenerateRoundKeys(key);

            for (int i = 0; i < text.Length; i++)
            {
                var bitArray = new BitArray(new byte[] { text[i] });
                Mix(ref bitArray, P8_TEXT_MIX_TEMPLATE);
                Round(bitArray, encrypt ? keys.roundKey1 : keys.roundKey2);
                Swap(bitArray);
                Round(bitArray, encrypt ? keys.roundKey2 : keys.roundKey1);
                Mix(ref bitArray, P8_TEXT_MIX_TEMPLATE_FINAL);
                bitArray.CopyTo(text, i);
            }

            return text;
        }

        /// <summary>
        /// S-DES encryption algorithm.<br/>
        /// Asymptotics: O(n)/>.Length.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="key">10-bit secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Encrypt(string text, string key)
        {
            if (text == null ||  key == null) throw new ArgumentNullException("Argument can't be null");
            if (key.Length != 10) throw new ArgumentException("Key length must be 10");

            var bitKey = StringToBitArray(key);

            return Encoding.Unicode.GetString(Chipher(Encoding.Unicode.GetBytes(text), bitKey, true));
        }

        /// <summary>
        /// S-DES decryption algorithm.<br/>
        /// Asymptotics: O(n)/>.Length.
        /// </summary>
        /// <param name="text">Text to decode.</param>
        /// <param name="key">10-bit secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Decrypt(string text, string key)
        {
            if (text == null || key == null) throw new ArgumentNullException("Argument can't be null");
            if (key.Length != 10) throw new ArgumentException("Key length must be 10");

            var bitKey = StringToBitArray(key);

            return Encoding.Unicode.GetString(Chipher(Encoding.Unicode.GetBytes(text), bitKey, false));
        }
    }
}
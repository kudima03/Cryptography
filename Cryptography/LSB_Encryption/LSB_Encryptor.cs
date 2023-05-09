using System.Collections;
using System.Drawing;


namespace Cryptography.LSB_Encryption
{
    internal class LSB_Encryptor
    {
        private static BitArray ByteToBitArray(byte value)
        {
            var bitArray = new BitArray(8);
            for (var i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = ((value >> i) & 1) == 1;
            }
            return bitArray;
        }

        private static byte BitArrayToByte(BitArray bitArray)
        {
            if (bitArray.Length != 8) throw new ArgumentException("BitArray length must be 8");
            byte num = 0;
            for (byte i = 0; i < bitArray.Count; i++)
            {
                num += (byte)((bitArray[i] ? 1 : 0) << i);
            }
            return num;
        }

        private static BitArray Int32ToBitArray(int value)
        {
            var bitArray = new BitArray(32);
            for (int i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = (value & 1) != 0;
                value >>= 1;
            }
            return bitArray;
        }

        private static int BitArrayToInt32(BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");
            int num = 0;
            for (int i = 0; i < bitArray.Length; i++)
            {
                num += (bitArray[i] ? 1 : 0) << i;
            }
            return num;
        }

        private static Color WriteByteToPixel(Color color, byte data)
        {
            var colorBits = Int32ToBitArray(color.ToArgb());
            var dataBits = ByteToBitArray(data);
            var dataBitsCounter = 0;
            for (int i = 5; i <= 7; i++)
            {
                colorBits[i] = dataBits[dataBitsCounter++];
            }

            for (int j = 13; j <= 15; j++)
            {
                colorBits[j] = dataBits[dataBitsCounter++];
            }

            for (int j = 22; j <= 23; j++)
            {
                colorBits[j] = dataBits[dataBitsCounter++];
            }

            return Color.FromArgb(BitArrayToInt32(colorBits));
        }

        private static byte ReadByteFromPixel(Color color)
        {
            var bits = new BitArray(8);
            var bitsCounter = 0;
            var pixel = Int32ToBitArray(color.ToArgb());

            for (int j = 5; j <= 7; j++)
            {
                bits[bitsCounter++] = pixel[j];
            }

            for (int j = 13; j <= 15; j++)
            {
                bits[bitsCounter++] = pixel[j];
            }

            for (int j = 22; j <= 23; j++)
            {
                bits[bitsCounter++] = pixel[j];
            }
            return BitArrayToByte(bits);
        }

        public static void WriteData(Bitmap container, byte[] data)
        {
            var dataLength = BitConverter.GetBytes(data.Length);
            for (int i = 0; i < dataLength.Length; i++)
            {
                var pixel = container.GetPixel(i, 0);
                var modifiedPixel = WriteByteToPixel(pixel, dataLength[i]);
                container.SetPixel(i, 0, modifiedPixel);
            }

            var dataLengthCounter = 0;
            for (int i = 0; i < container.Height; i++)
            {
                for (int j = 0; j < container.Width; j++)
                {
                    if (i == 0 && j < dataLength.Length) continue;

                    if (dataLengthCounter == data.Length) return;

                    var pixel = container.GetPixel(j, i);
                    var modifiedPixel = WriteByteToPixel(pixel, data[dataLengthCounter++]);
                    container.SetPixel(j, i, modifiedPixel);
                }
            }
        }

        public static byte[] ReadData(Bitmap container)
        {
            var dataLengthBytes = new byte[4];
            for (int i = 0; i < dataLengthBytes.Length; i++)
            {
                dataLengthBytes[i] = ReadByteFromPixel(container.GetPixel(i, 0));
            }

            var dataLength = BitConverter.ToInt32(dataLengthBytes, 0);

            var data = new byte[dataLength];

            var dataLengthCounter = 0;
            for (int i = 0; i < container.Height; i++)
            {
                for (int j = 0; j < container.Width; j++)
                {
                    if (i == 0 && j < dataLengthBytes.Length) continue;

                    if (dataLengthCounter == data.Length) return data;

                    data[dataLengthCounter++] = ReadByteFromPixel(container.GetPixel(j, i));
                }
            }
            throw new Exception();
        }
    }
}

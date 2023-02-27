using System.Text;

namespace Cryptography.RailFenceEncryption
{
    public class RailFence
    {
        /// <summary>
        /// RailFence encryption algorithm.<br/>
        /// Asymptotics: O(n).
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="key">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Encrypt(string text, int key)
        {
            if (text == null) throw new ArgumentNullException("Text can't be null.");
            if (key <= 0) throw new ArgumentException("Key must be greater than 0.");
            if (text == string.Empty) return "";
            if (key == 1) return text;
            if (key >= text.Length) return text;

            var lines = new StringBuilder[key];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new StringBuilder(text.Length / key);
            }

            int stringIndex = 0;

            bool directionDown = false;

            for (int i = 0; i < text.Length; i++)
            {
                lines[stringIndex].Append(text[i]);

                if (stringIndex == 0 || stringIndex == key - 1)
                {
                    directionDown = !directionDown;
                }

                if (directionDown)
                {
                    stringIndex++;
                }
                else
                {
                    stringIndex--;
                }
            }

            var encryptedStr = new StringBuilder(text.Length);

            foreach (var line in lines)
            {
                encryptedStr.Append(line);
            }
            return encryptedStr.ToString();
        }

        /// <summary>
        /// RailFence decryption algorithm.<br/>
        /// Asymptotics: O(2n) => O(n).
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="key">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Decrypt(string text, int key)
        {
            if (text == null) throw new ArgumentNullException("Text can't be null.");
            if (key <= 0) throw new ArgumentException("Key must be greater than 0.");
            if (text == string.Empty) return "";
            if (key == 1) return text;
            if (key >= text.Length) return text;

            int[] linesLenght = Enumerable.Repeat(0, key).ToArray();

            int currentLine = 0;
            bool directionDown = false;

            for (int i = 0; i < text.Length; i++)
            {
                linesLenght[currentLine]++;

                if (currentLine == 0 || currentLine == key - 1)
                {
                    directionDown = !directionDown;
                }

                if (directionDown)
                {
                    currentLine++;
                }
                else
                {
                    currentLine--;
                }
            }

            var lines = new string[key];

            int prevSubstrLastIndex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = text.Substring(prevSubstrLastIndex, linesLenght[i]);

                prevSubstrLastIndex += linesLenght[i];
            }

            var decryptedStr = new StringBuilder(text.Length);

            currentLine = 0;

            directionDown = false;

            int[] lineLastSymbolIndex = Enumerable.Repeat(0, key).ToArray();

            for (int i = 0; i < text.Length; i++)
            {
                decryptedStr.Append(lines[currentLine][lineLastSymbolIndex[currentLine]]);

                lineLastSymbolIndex[currentLine]++;

                if (currentLine == 0 || currentLine == key - 1)
                {
                    directionDown = !directionDown;
                }

                if (directionDown)
                {
                    currentLine++;
                }
                else
                {
                    currentLine--;
                }
            }
            return decryptedStr.ToString();
        }


        public static void Demo()
        {
            Console.WriteLine(" Rail Fence ");
            Console.WriteLine("Введите текст:");
            var str = Console.ReadLine();
            var encrypted = Encrypt(str, 10);
            Console.WriteLine("Зашифрованный текст:");
            Console.WriteLine(encrypted);
            Console.WriteLine("Расшифрованный  текст:");
            Console.WriteLine(Decrypt(encrypted, 10));
        }
    }
}

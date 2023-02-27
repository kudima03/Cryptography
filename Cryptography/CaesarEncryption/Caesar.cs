using System.Text.RegularExpressions;

namespace Cryptography.CaesarEncryption
{
    public class Caesar
    {
        private const short SYMBOLS_AMOUNT = 26;

        private static string Chipher(string text, int shift, bool encrypt)
        {
            if (text == null) throw new ArgumentNullException($"Parameter {nameof(text)} can't be null.");
            if (!Regex.IsMatch(text, @"^[a-zA-Z ]+$")) throw new ArgumentException("Only letters A-Z are allowed.");

            var buffer = text.ToUpper().ToCharArray();

            for (int i = 0; i < buffer.Length; i++)
            {
                var letter = buffer[i];

                if (encrypt)
                {
                    letter = (char)(letter + shift);
                }
                else
                {
                    letter = (char)(letter - shift);
                }

                if (letter > 'Z')
                {
                    letter = (char)(letter - SYMBOLS_AMOUNT);
                }
                else if (letter < 'A')
                {
                    letter = (char)(letter + SYMBOLS_AMOUNT);
                }
                buffer[i] = letter;
            }
            return new string(buffer);
        }

        /// <summary>
        /// Caesar encryption algorithm.<br/>
        /// Asymptotics: O(n)
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="shift">Secret shift.</param>
        /// <returns>Returns encrypted string according to <paramref name="shift"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Encrypt(string text, int shift) => Chipher(text, shift, true);

        /// <summary>
        /// RailFence decryption algorithm.<br/>
        /// Asymptotics: O(n)
        /// </summary>
        /// <param name="text">Text to decode.</param>
        /// <param name="shift">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="shift"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Decrypt(string text, int shift) => Chipher(text, shift, false);


        public static void Demo()
        {
            Console.WriteLine("Введите текст:");
            var str = Console.ReadLine();
            var encrypted = Encrypt(str, 10);
            Console.WriteLine(Decrypt(encrypted, 10));
        }
    }
}

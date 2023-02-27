namespace Cryptography.MultiplyMethodEncryption
{
    public class MultiplyMethod
    {
        private static readonly int[] _encryptKeys;

        private static readonly int[] _decryptKeys;

        const int UNICODE_SUPPORTED_SYMBOLS_AMOUNT = 1114112;

        static MultiplyMethod()
        {
            var keys = MultiplyMethodKeysGenerator.GetStoredKeys();
            _encryptKeys = keys.encryptKeys;
            _decryptKeys = keys.decryptKeys;
        }

        private static string Chipher(string text, int key)
        {
            if (text == null) throw new ArgumentNullException($"{nameof(text)} parameter can't be null.");

            if (key <= 0) throw new ArgumentException("Key can't be less than zero.");

            var resultText = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                resultText[i] = (char)(text[i] * key % UNICODE_SUPPORTED_SYMBOLS_AMOUNT);
            }

            return new string(resultText);
        }

        /// <summary>
        /// MultiplyMethod encryption algorithm.<br/>
        /// Asymptotics: O(n).
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <returns>Returns encrypted string with decryption key.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static (string encryptedText, int decryptKey) Encrypt(string text)
        {
            var random = new Random();
            var keysIndex = random.Next(0, _encryptKeys.Length);
            return (Chipher(text, _encryptKeys[keysIndex]), _decryptKeys[keysIndex]);
        }

        /// <summary>
        /// MultiplyMethod decryption algorithm.<br/>
        /// Asymptotics: O(n).
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="key">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Decrypt(string text, int key) => Chipher(text, key);

        public static void Demo()
        {
            Console.WriteLine("Введите текст:");
            var str = Console.ReadLine();
            var pair = Encrypt(str);
            Console.WriteLine(pair.encryptedText);
            Console.WriteLine(Decrypt(pair.encryptedText, pair.decryptKey));
        }
    }
}

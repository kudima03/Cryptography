using System.Text;

namespace Cryptography.KeyPhraseEncryption
{
    public class KeyPhrase
    {
        private static int[] GetSortingTemplate(string keyPhrase, bool encryption)
        {
            var orderedKeyPhrase = string.Concat(keyPhrase.Order());

            var sortingTemplate = new int[keyPhrase.Length];

            for (int i = 0; i < keyPhrase.Length; i++)
            {
                int repeats = 0;
                for (int j = 0; j < i; j++)
                {
                    if (keyPhrase[i] == keyPhrase[j])
                    {
                        repeats++;
                    }
                }
                sortingTemplate[i] = orderedKeyPhrase.IndexOf(keyPhrase[i]) + repeats;
            }

            if (!encryption)
            {
                var buf = Enumerable.Range(0, sortingTemplate.Length).ToArray();

                Array.Sort(sortingTemplate, buf);

                sortingTemplate = buf;
            }
            return sortingTemplate;
        }

        private static string Chipher(string text, string keyPhrase, bool encrypt)
        {
            if (text == null || keyPhrase == null) throw new ArgumentNullException("Parameter can't be null.");
            if (text == string.Empty) return "";
            if (keyPhrase == string.Empty) return text;

            // Append specified symbols (text length must be >= and aliquot to keyPhrase length.
            if (text.Length < keyPhrase.Length)
            {
                text += new string('~', keyPhrase.Length - text.Length);
            }
            else if (text.Length % keyPhrase.Length != 0)
            {
                text += new string('~', keyPhrase.Length - text.Length % keyPhrase.Length);
            }
            //

            var sortingTemplate = GetSortingTemplate(keyPhrase, encrypt);

            var strings = new string[text.Length / keyPhrase.Length];

            Parallel.For(0, strings.Length, (i) =>
            {
                var blockSequence = text.Substring(i * keyPhrase.Length, keyPhrase.Length).ToCharArray();
                Array.Sort(sortingTemplate.Clone() as int[], blockSequence);
                strings[i] = new string(blockSequence);
            });

            var result = new StringBuilder(text.Length);

            foreach (var item in strings)
            {
                result.Append(item);
            }

            if (!encrypt) result.Replace("~", "");

            return result.ToString();
        }

        /// <summary>
        /// Key phrase encryption algorithm.<br/>
        /// Asymptotics: O(n)
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="keyPhrase">Secret key phrase.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Encrypted text according to key.</returns>
        public static string Encrypt(string text, string keyPhrase) => Chipher(text, keyPhrase, true);

        /// <summary>
        /// Key phrase decryption algorithm.<br/>
        /// Asymptotics: O(n)
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="keyPhrase">Secret key phrase.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Decrypted text according to key.</returns>
        public static string Decrypt(string text, string keyPhrase) => Chipher(text, keyPhrase, false);


        public static void Demo()
        {
            Console.WriteLine(" Key Phrase ");
            Console.WriteLine("Введите текст:");
            var str = Console.ReadLine();
            Console.WriteLine("Введите фразу:");
            var phrase = Console.ReadLine();
            var encrypted = Encrypt(str, phrase);
            Console.WriteLine("Зашифрованный текст:");
            Console.WriteLine(encrypted);
            Console.WriteLine("Расшифрованный текст:");
            Console.WriteLine(Decrypt(encrypted, phrase));
        }
    }
}

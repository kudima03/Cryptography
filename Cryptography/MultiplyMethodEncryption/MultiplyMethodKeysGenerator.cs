namespace Cryptography.MultiplyMethodEncryption;

public class MultiplyMethodKeysGenerator
{
    private const string ENCRYPT_KEYS_FILE_NAME = "Encrypt keys.txt";
    private const string DECRYPT_KEYS_FILE_NAME = "Decrypt keys.txt";

    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private static int[] GenerateEncryptKeys(int keysAmount, int symbolsAmount)
    {
        var keys = new int[keysAmount];

        var foundKeys = 0;

        var i = 0;

        while (foundKeys != keysAmount)
        {
            if (GCD(symbolsAmount, i) == 1) keys[foundKeys++] = i;
            i++;
        }

        return keys;
    }

    private static int[] GenerateDecryptKeys(int[] encryptKeys, int symbolsAmount)
    {
        var decryptKeys = new int[encryptKeys.Length];

        for (var i = 0; i < decryptKeys.Length; i++)
        {
            var key = symbolsAmount / encryptKeys[i];

            while (key * encryptKeys[i] % symbolsAmount != 1) key++;
            decryptKeys[i] = key;
        }

        return decryptKeys;
    }

    public static void GenerateKeys(int symbolsAmount, int keysAmount)
    {
        var encryptKeys = GenerateEncryptKeys(symbolsAmount, keysAmount);
        var decryptKeys = GenerateDecryptKeys(encryptKeys, symbolsAmount);

        using (var writer = new StreamWriter(File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(),
                   "MultiplyMethodEncryption", ENCRYPT_KEYS_FILE_NAME))))
        {
            foreach (var item in encryptKeys) writer.WriteLine(item);
        }

        using (var writer = new StreamWriter(File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(),
                   "MultiplyMethodEncryption", DECRYPT_KEYS_FILE_NAME))))
        {
            foreach (var item in decryptKeys) writer.WriteLine(item);
        }
    }

    public static (int[] encryptKeys, int[] decryptKeys) GetStoredKeys()
    {
        var encryptKeysList = new List<int>();
        var decryptKeysList = new List<int>();

        using (var reader = new StreamReader(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(),
                   "MultiplyMethodEncryption", ENCRYPT_KEYS_FILE_NAME))))
        {
            while (!reader.EndOfStream) encryptKeysList.Add(int.Parse(reader.ReadLine()));
        }

        using (var reader = new StreamReader(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(),
                   "MultiplyMethodEncryption", DECRYPT_KEYS_FILE_NAME))))
        {
            while (!reader.EndOfStream) decryptKeysList.Add(int.Parse(reader.ReadLine()));
        }

        return (encryptKeysList.ToArray(), decryptKeysList.ToArray());
    }
}
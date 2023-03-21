using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cryptography.RotatingGridEncryption;

public static class RotatingGrid
{
    private static char[][] ChooseAppropriateGrid(string text)
    {
        if (text.Length <= 16)
            return new[]
            {
                new[] { '1', ' ', ' ', ' ' },
                new[] { ' ', ' ', ' ', '2' },
                new[] { ' ', ' ', '4', ' ' },
                new[] { ' ', '3', ' ', ' ' }
            };
        /*            else if (text.Length <= 24)
                    {
                        return new char[][]
                        {
                            new char[] { '1', '2', '3', '4', ' '},
                            new char[] { ' ', ' ', '6', '5', ' '},
                            new char[] { ' ', ' ', '7', ' ', ' '},
                            new char[] { ' ', ' ', ' ', ' ', ' '},
                            new char[] { ' ', ' ', ' ', ' ', ' '},
                        };
                    }*/
        if (text.Length <= 36)
            return new[]
            {
                new[] { '1', '2', '3', '4', '5', ' ' },
                new[] { ' ', ' ', '7', '8', '6', ' ' },
                new[] { ' ', ' ', ' ', '9', ' ', ' ' },
                new[] { ' ', ' ', ' ', ' ', ' ', ' ' },
                new[] { ' ', ' ', ' ', ' ', ' ', ' ' },
                new[] { ' ', ' ', ' ', ' ', ' ', ' ' }
            };
        throw new ArgumentException("Input text is too large.");
    }

    private static Coordinate[] GetStartCoordinates(char[][] grid)
    {
        var coordinates = new List<Coordinate>();
        for (var x = 0; x < grid.Length; x++)
        for (var y = 0; y < grid.First().Length; y++)
            if (grid[x][y] != ' ')
                coordinates.Add(new Coordinate(x, y, grid[x][y]));
        return coordinates.OrderBy(x => x.Value).ToArray();
    }

    private static string DecryptMessage(string encryptedStr, char[][] grid)
    {
        var coordinates = GetStartCoordinates(grid);

        var charGrid = new char[grid.Length][];

        for (var i = 0; i < grid.Length; i++)
            charGrid[i] = encryptedStr.Substring(i * grid.Length, grid.Length).ToCharArray();

        var decryptedStr = new StringBuilder(encryptedStr);

        for (var j = 0; j < coordinates.Length; j++)
        {
            var x = coordinates[j].X;
            var y = coordinates[j].Y;

            for (var i = 0; i < 4; i++)
            {
                decryptedStr[j + coordinates.Length * i] = charGrid[x][y];
                var buf = x;
                x = grid.Length - 1 - y;
                y = buf;
            }
        }

        return decryptedStr.Replace("~", "").ToString();
    }

    private static void EncryptMessage(char[][] grid, string text)
    {
        var coordinates = GetStartCoordinates(grid);

        for (var j = 0; j < coordinates.Length; j++)
        {
            var x = coordinates[j].X;
            var y = coordinates[j].Y;

            for (var i = 0; i < 4; i++)
            {
                var index = j + coordinates.Length * i;
                var a = text[index];
                grid[x][y] = a;

                var buf = x;
                x = grid.Length - 1 - y;
                y = buf;
            }
        }
    }

    private static string Chipher(string text, char[][] grid)
    {
        if (text == null || grid == null) throw new ArgumentNullException();

        if (text.Length < grid.Length * grid.Length) text += new string('~', grid.Length * grid.Length - text.Length);

        EncryptMessage(grid, text);

        var result = new StringBuilder(text.Length);

        foreach (var item in grid) result.Append(item);

        return result.ToString();
    }

    /// <summary>
    ///     Rotating grid encryption algorithm.<br />
    ///     Asymptotics: O(n) where n - <paramref name="keyGrid" />.Length.
    /// </summary>
    /// <param name="text">Text to encode.</param>
    /// <param name="grid">Secret key.</param>
    /// <returns>Returns encrypted string according to <paramref name="keyGrid" />.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static (string encryptedStr, char[][] grid) Encrypt(string text)
    {
        if (text == null) throw new ArgumentNullException();
        var grid = ChooseAppropriateGrid(text);
        var gridCopy = ChooseAppropriateGrid(text);
        return (Chipher(text, gridCopy), grid);
    }

    /// <summary>
    ///     RailFence decryption algorithm.<br />
    ///     Asymptotics: O(n) where n - <paramref name="keyGrid" />.Length.
    /// </summary>
    /// <param name="text">Text to encode.</param>
    /// <param name="keyGrid">Secret key.</param>
    /// <returns>Returns encrypted string according to <paramref name="keyGrid" />.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string Decrypt(string text, char[][] keyGrid)
    {
        if (text == null) throw new ArgumentNullException();
        return DecryptMessage(text, keyGrid);
    }

    public static void Demo()
    {
        Console.WriteLine(" Rotating Grid ");
        Console.WriteLine("Введите текст:");
        var testText = Console.ReadLine();
        var encrypted = Encrypt(testText);
        Console.WriteLine("Зашифрованный текст:");
        Console.WriteLine(encrypted.encryptedStr);
        Console.WriteLine("Расшифрованный текст:");
        Console.WriteLine(Decrypt(encrypted.encryptedStr, encrypted.grid));
    }

    private struct Coordinate
    {
        public Coordinate(int x, int y, char value)
        {
            X = x;
            Y = y;
            Value = value;
        }

        public readonly int X;
        public readonly int Y;
        public readonly char Value;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            var other = (Coordinate)obj;
            return X == other.X && Y == other.Y;
        }
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cryptography.RotatingGridEncryption
{
    public static class RotatingGrid
    {
        private struct Coordinate
        {
            public Coordinate(int x, int y, char value)
            {
                X = x;
                Y = y;
                Value = value;
            }
            public int X;
            public int Y;
            public char Value;
            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                var other = (Coordinate)obj;
                return X == other.X && Y == other.Y;
            }
        }

        private static char[][] ChooseAppropriateGrid(string text)
        {
            if (text.Length <= 16)
            {
                return new char[][]
                    {
                        new char[] { '1', ' ', ' ', ' '},
                        new char[] { ' ', ' ', ' ', '2'},
                        new char[] { ' ', ' ', '4', ' '},
                        new char[] { ' ', '3', ' ', ' '},
                    };
            }
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
            else if (text.Length <= 36)
            {
                return new char[][]
                {
                    new char[] { '1', '2', '3', '4', '5', ' '},
                    new char[] { ' ', ' ', '7', '8', '6', ' '},
                    new char[] { ' ', ' ', ' ', '9', ' ', ' '},
                    new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                    new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                    new char[] { ' ', ' ', ' ', ' ', ' ', ' '},
                };
            }
            else
            {
                throw new ArgumentException("Input text is too large.");
            }
        }

        private static Coordinate[] GetStartCoordinates(char[][] grid)
        {
            var coordinates = new List<Coordinate>();
            for (int x = 0; x < grid.Length; x++)
            {
                for (int y = 0; y < grid.First().Length; y++)
                {
                    if (grid[x][y] != ' ')
                    {
                        coordinates.Add(new Coordinate(x, y, grid[x][y]));
                    }
                }
            }
            return coordinates.OrderBy(x => x.Value).ToArray();
        }

        private static string DecryptMessage(string encryptedStr, char[][] grid)
        {
            var coordinates = GetStartCoordinates(grid);

            var charGrid = new char[grid.Length][];

            for (int i = 0; i < grid.Length; i++)
            {
                charGrid[i] = encryptedStr.Substring(i * grid.Length, grid.Length).ToCharArray();
            }

            var decryptedStr = new StringBuilder(encryptedStr);

            for (int j = 0; j < coordinates.Length; j++)
            {
                int x = coordinates[j].X;
                int y = coordinates[j].Y;

                for (int i = 0; i < 4; i++)
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

            for (int j = 0; j < coordinates.Length; j++)
            {
                int x = coordinates[j].X;
                int y = coordinates[j].Y;

                for (int i = 0; i < 4; i++)
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

            if (text.Length < grid.Length * grid.Length)
            {
                text += new string('~', grid.Length * grid.Length - text.Length);
            }

            EncryptMessage(grid, text);

            var result = new StringBuilder(text.Length);

            foreach (var item in grid)
            {
                result.Append(item);
            }

            return result.ToString();
        }

        /// <summary>
        /// Rotating grid encryption algorithm.<br/>
        /// Asymptotics: O(n) where n - <paramref name="keyGrid"/>.Length.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="grid">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="keyGrid"/>.</returns>
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
        /// RailFence decryption algorithm.<br/>
        /// Asymptotics: O(n) where n - <paramref name="keyGrid"/>.Length.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="keyGrid">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="keyGrid"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Decrypt(string text, char[][] keyGrid)
        {
            if (text == null) throw new ArgumentNullException();
            return DecryptMessage(text, keyGrid);
        }

        public static void Demo()
        {
            var testText = Console.ReadLine();

            var encrypted = Encrypt(testText);

            Console.WriteLine(encrypted.encryptedStr);

            Console.WriteLine(Decrypt(encrypted.encryptedStr, encrypted.grid));
        }
    }
}

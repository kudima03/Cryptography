using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return X == other.X && Y == other.Y && Value == other.Value;
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
            return coordinates.ToArray();
        }

        private static void FillCoordinates(char[][] grid)
        {
            var coordinates = GetStartCoordinates(grid);

            foreach (var coordinate in coordinates)
            {
                int x = grid.Length - 1 - coordinate.Y;
                int y = coordinate.X;

                for (int i = 0; i < 3; i++)
                {
                    if (grid[x][y] != coordinate.Value && grid[x][y] != ' ') throw new ArgumentException("Invalid input key.");

                    grid[x][y] = coordinate.Value;

                    var buf = x;
                    x = grid.Length - 1 - y;
                    y = buf;
                }
            }
        }

        private static string Chipher(string text, char[][] grid, bool encrypt)
        {
            if (text == null || grid == null) throw new ArgumentNullException();

            FillCoordinates(grid);

            if (text.Length < grid.Length * grid.First().Length && encrypt)
            {
                text += new string('~', grid.Length * grid.First().Length - text.Length);
            }

            var strings = new string[text.Length / grid.Length];

            Parallel.For(0, grid.Length, (i) =>
            {
                var blockSequence = text.Substring(i * grid.Length, grid.Length).ToCharArray();

                if (encrypt)
                {
                    Array.Sort(grid[i], blockSequence);
                }
                else
                {
                    var sortingTemplate = Enumerable.Range(0, grid.Length).ToArray();
                    Array.Sort(grid[i], sortingTemplate);
                    Array.Sort(sortingTemplate, blockSequence);
                }
                strings[i] = new string(blockSequence);
            });

            var result = new StringBuilder(text.Length);

            foreach (var item in strings)
            {
                result.Append(item);
            }

            if (!encrypt)
            {
                result.Replace("~", "");
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
        public static string Encrypt(string text, char[][] keyGrid) => Chipher(text, keyGrid, true);

        /// <summary>
        /// RailFence decryption algorithm.<br/>
        /// Asymptotics: O(n) where n - <paramref name="keyGrid"/>.Length.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <param name="keyGrid">Secret key.</param>
        /// <returns>Returns encrypted string according to <paramref name="keyGrid"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Decrypt(string text, char[][] keyGrid) => Chipher(text, keyGrid, false);
    }
}

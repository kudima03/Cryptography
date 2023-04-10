namespace Cryptography.RSA
{
    public class PrimesGenerator
    {
        private readonly OptimizedSegmentedWheel235 _algorithm;

        public PrimesGenerator(long amount)
        {
            _algorithm = new OptimizedSegmentedWheel235(amount);
        }

        public void Generate(StreamWriter writer)
        {
            var list = new List<long>();
            _algorithm.ListPrimes(list.Add);
            foreach (var item in list.SkipWhile(x => x < 100))
            {
                writer.WriteLine(item);
            }
        }
    }
}

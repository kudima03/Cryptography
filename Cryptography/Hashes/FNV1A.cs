namespace Cryptography.Hashes;

public static class FNV1A
{
    private const uint FNV_prime = 0x1000193;
    private const uint FNV_offset_basic = 0x811C9DC5;

    public static uint Hash(string value)
    {
        var hash = FNV_offset_basic;
        for (var i = 0; i < value.Length; i++)
        {
            hash ^= value[i];
            hash *= FNV_prime;
        }

        return hash;
    }
}
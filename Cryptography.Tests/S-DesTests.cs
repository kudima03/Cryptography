namespace Cryptography.Tests;

[TestClass]
public class SDesTests
{
    private readonly string _largeTextExample;

    public SDesTests()
    {
        _largeTextExample =
            "asdasdasdasdasd" /*File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Text examples", "Text example.txt"))*/
            ;
    }

    [TestMethod]
    public void TestEncryptDecryptEqualsUnicode()
    {
        var key = "1001010011";
        var encryptedStr = SimplifiedDES.SimplifiedDES.Encrypt(_largeTextExample, key);
        Assert.AreEqual(SimplifiedDES.SimplifiedDES.Decrypt(encryptedStr, key), _largeTextExample);
    }

    [TestMethod]
    public void TestEncryptNullStringParameter()
    {
        Assert.ThrowsException<ArgumentNullException>(() => SimplifiedDES.SimplifiedDES.Encrypt(null, null));
    }

    [TestMethod]
    public void TestDecryptNullStringParameter()
    {
        Assert.ThrowsException<ArgumentNullException>(() => SimplifiedDES.SimplifiedDES.Decrypt(null, null));
    }

    [TestMethod]
    public void TestEncryptWrongKey()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            SimplifiedDES.SimplifiedDES.Encrypt(_largeTextExample, "10100"));
    }

    [TestMethod]
    public void TestDecryptWrongKey()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            SimplifiedDES.SimplifiedDES.Decrypt(_largeTextExample, "10100"));
    }
}
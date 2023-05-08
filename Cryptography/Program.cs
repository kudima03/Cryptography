using Cryptography.DigitalSignature;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Введите текст документа для подписи:");

        var document = Console.ReadLine();

        var signaturePerformer = new RsaSignature();
        
        var digitalSign = signaturePerformer.GenerateSign(document);

        var validationResult = signaturePerformer.ValidateSign(document, digitalSign.signValue, digitalSign.openKey, digitalSign.r);
    }
}
namespace ProductManagement.Services
{
    public interface IEncryptionService
    {
        string EncryptText(string plainText, string encryptionPrivateKey = "");
    }
}

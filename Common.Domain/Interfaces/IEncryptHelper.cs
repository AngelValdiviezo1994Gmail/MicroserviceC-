
namespace Common.Domain.Interfaces
{
    public interface IEncryptHelper
    {
        string Encrypt(string input);

        string EncryptAES(string plainText);

        string DecryptAES(string encryptedText);
    }
}

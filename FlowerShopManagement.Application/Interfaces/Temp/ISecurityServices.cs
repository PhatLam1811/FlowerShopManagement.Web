using System.Security.Cryptography;

namespace FlowerShopManagement.Application.Interfaces.Temp;

public interface ISecurityServices
{
    public bool IsValidEmail(string input);

    public bool IsValidPhoneNumber(string input);
    
    public string Encrypt(MD5 md5Hash, string plainText);
    
    public bool VerifyEmail(string emailAddress);
    
    public string CodeGenerator();
}

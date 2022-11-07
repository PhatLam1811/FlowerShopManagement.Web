namespace FlowerShopManagement.Application.Interfaces.Temp;

public interface ISecurityServices
{
    public bool IsValidEmail(string input);
    public bool IsValidPhoneNumber(string input);
    public string Encrypt(string plainText);
    public string Decrypt(string cipherText);
    public bool VerifyEmail(string emailAddress);
    public string CodeGenerator();
}

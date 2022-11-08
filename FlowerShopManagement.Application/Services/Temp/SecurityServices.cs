//using System.Text.RegularExpressions;
//using System.Security.Cryptography;
//using System.Text;
//using FlowerShopManagement.Application.Interfaces.Temp;

//namespace FlowerShopManagement.Application.Services.Temp;

//public class SecurityServices : ISecurityServices
//{
//    // This constant is used to determine the format of normal phone numbers.
//    private const string Motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

//    // This constant is used to determine the keysize of the encryption algorithm in bits.
//    // We divide this by 8 within the code below to get the equivalent number of bytes.
//    private const int Keysize = 256;

//    // This constant determines the number of iterations for the password bytes generation function.
//    private const int DerivationIterations = 1000;

//    private const string passPhrase = "erqiowunfeioausniufiwenqfuwnefiunasdf";

//    public bool IsValidEmail(string input)
//    {
//        var trimmedEmail = input.Trim();

//        if (trimmedEmail.EndsWith("."))
//        {
//            return false;
//        }
//        try
//        {
//            var addr = new System.Net.Mail.MailAddress(input);
//            return addr.Address == trimmedEmail;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public bool IsValidPhoneNumber(string input)
//    {
//        if (input != null) return Regex.IsMatch(input, Motif);

//        return false;
//    }

//    public string Encrypt(MD5 md5Hash , string plainText)
//    {
//        // Convert the input string to a byte array and compute the hash.
//        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
//        StringBuilder sBuilder = new StringBuilder();
//        for (int i = 0; i < data.Length; i++)
//        {
//            sBuilder.Append(data[i].ToString("x2"));
//        }
//        return sBuilder.ToString();
//    }

//    private byte[] Generate256BitsOfRandomEntropy()
//    {
//        var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
//        using (var rngCsp = RandomNumberGenerator.Create())
//        {
//            // Fill the array with cryptographically secure random bytes.
//            rngCsp.GetBytes(randomBytes);
//        }
//        return randomBytes;
//    }

//    public bool VerifyEmail(string emailAddress)
//    {
//        throw new NotImplementedException();
//    }

//    public string CodeGenerator()
//    {
//        var randomNumber = new byte[6];
//        string refreshToken = "";

//        using (var rng = RandomNumberGenerator.Create())
//        {
//            rng.GetBytes(randomNumber);
//            refreshToken = Convert.ToBase64String(randomNumber);
//        }

//        return refreshToken;
//    }
//}

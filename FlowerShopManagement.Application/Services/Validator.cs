using System.Security.Cryptography;
using System.Text;

namespace FlowerShopManagement.Application.Services;

public class Validator
{
    public static string GenerateValidationCode()
    {
        using RNGCryptoServiceProvider rg = new();
        byte[] rno = new byte[5];
        rg.GetBytes(rno);
        int randomvalue = BitConverter.ToInt32(rno, 0);

        return randomvalue.ToString();
    }

    public static string MD5Hash(string input)
    {
        // Calculate MD5 hash from input
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Convert byte array to hex string
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }

        return sb.ToString();
    }
}

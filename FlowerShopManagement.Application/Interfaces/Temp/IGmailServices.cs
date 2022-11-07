namespace FlowerShopManagement.Application.Interfaces.Temp;

public interface IGmailServices
{
    public void SendGmail(string fromEmail, string toEmail, string content);
}

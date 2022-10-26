namespace FlowerShopManagement.Application.Interfaces;

public interface IAuthenticationServices
{
    public bool Register(string email, string phoneNumber, string pasword);
    public bool Login(string username, string password);
    public void Logout();
}

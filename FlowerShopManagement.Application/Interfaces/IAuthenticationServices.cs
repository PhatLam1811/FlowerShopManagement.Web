namespace FlowerShopManagement.Application.Interfaces;

public interface IAuthenticationServices
{
    public bool Register(string username, string pasword, string reEnter);
    public bool Login(string username, string password);
    public void Logout();
}

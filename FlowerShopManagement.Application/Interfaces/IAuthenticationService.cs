using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using System.Security.Claims;

namespace FlowerShopManagement.Application.Interfaces;

public interface IAuthenticationService
{
    public Task<UserModel?> RegisterAsync(string email, string phoneNb, string password, Role? role = null);
    public Task<UserModel?> SignInAsync(string emailOrPhoneNb, string password);
    public Task<UserModel?> AuthenticateAsync(string id);
    public ClaimsPrincipal CreateUserClaims(string id, string role);
    public User? GetUser();
}

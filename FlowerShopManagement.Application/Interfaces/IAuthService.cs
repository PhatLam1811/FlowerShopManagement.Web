using FlowerShopManagement.Application.Models;
using Microsoft.AspNetCore.Http;

namespace FlowerShopManagement.Application.Interfaces;

public interface IAuthService
{
    public Task<bool> RegisterAsync(HttpContext httpContext, string email, string phoneNb, string password);
    public Task<bool> SignInAsync(HttpContext httpContext, string emailOrPhoneNb, string password);
    public Task<bool> SignOutAsync(HttpContext httpContext);
    public Task<UserModel> GetUserAsync(HttpContext httpContext);
    public string? GetUserId(HttpContext httpContext);
    public string? GetUserRole(HttpContext httpContext);
}

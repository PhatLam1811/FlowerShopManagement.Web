using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Application.Interfaces;

public interface IUserServices
{
    public Task<List<User>> GetAllUsers();
    public Task<User> GetUsersByUsername(string username);
    public Task<User> GetUserById(string id);
}

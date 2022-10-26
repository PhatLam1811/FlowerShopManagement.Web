using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface IUserServices
{
    public Task<List<User>> GetAllUsers();
    public Task<User> GetUsersByUsername(string username);
    public Task<User> GetUserByEmail(string email);
    public Task<User> GetUserByPhoneNumber(string phoneNumber);
    public Task<User> GetUserById(string id);
    public Task<bool> Add(User newUser);
    public Task<bool> Update(string id);
    public Task<bool> Delete(string id);
}

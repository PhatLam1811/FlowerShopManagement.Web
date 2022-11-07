using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces.Temp;

public interface IUserManagementServices
{
    public Task<List<User>> GetAllUsers();
    public Task<User> GetUsersByUsername(string username);
    public Task<User> GetUserByEmail(string email);
    public Task<User> GetUserByPhoneNumber(string phoneNumber);
    public Task<User> GetUserById(string id);
    public Task<bool> UpdateUserById(string id, User updatedUser);
    public Task<bool> DeleteUserById(string id);
    public Task<bool> Add(User newUser);
}
